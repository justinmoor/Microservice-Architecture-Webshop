using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Attributes;
using Minor.Nijn.WebScale.Commands;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.SharedTypes.BeheerService.Commands;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.BeheerService.Events;
using Sprinters.SharedTypes.BetaalService;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.BestellingBeheer.Listeners
{
    [CommandListener]
    public class BestellingListener
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly IBestellingDatamapper _datamapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IKlantDatamapper _klantDatamapper;
        private readonly ILogger _logger;

        public BestellingListener(IBestellingDatamapper datamapper, IKlantDatamapper klantDatamapper,
            ICommandPublisher commandPublisher,
            IEventPublisher eventPublisher)

        {
            _datamapper = datamapper;
            _klantDatamapper = klantDatamapper;
            _commandPublisher = commandPublisher;
            _eventPublisher = eventPublisher;
            _logger = NijnLogger.CreateLogger<BestellingListener>();
        }

        [Command(NameConstants.NieuweBestellingCommandQueue)]
        public async Task<int> PlaatsBestelling(NieuweBestellingCommand nieuweBestellingCommand)
        {
            var bestellingItems = new List<BestellingItem>();
            nieuweBestellingCommand.BesteldeArtikelen.ForEach(a =>
            {
                bestellingItems.Add(new BestellingItem {Artikelnummer = a.Artikelnummer, Aantal = a.Aantal});
            });

            var bestelling = new Bestelling
            {
                KlantId = nieuweBestellingCommand.KlantId,
                AdresRegel1 = nieuweBestellingCommand.AdresRegel1,
                AdresRegel2 = nieuweBestellingCommand.AdresRegel2,
                Postcode = nieuweBestellingCommand.Postcode,
                Plaats = nieuweBestellingCommand.Plaats,
                BesteldeArtikelen = bestellingItems
            };

            await _datamapper.Insert(bestelling);

            //get full bestelling so all data like artikel prices are filled in
            var fullBestelling = await _datamapper.GetBestelling(bestelling.Id);

            if (fullBestelling.GetTotaalPrijs + fullBestelling.Klant.KredietMetSales > NameConstants.KredietLimit)
            {
                fullBestelling.BestellingStatus = BestellingStatus.TerControleVoorSales;
            }
            else
            {
                fullBestelling.BestellingStatus = BestellingStatus.GereedVoorBehandeling;
                fullBestelling.Klant.KredietOver -= fullBestelling.GetTotaalPrijs;
            }

            fullBestelling.Klant.KredietMetSales += fullBestelling.GetTotaalPrijs;
            await _klantDatamapper.Update(fullBestelling.Klant);

            await _datamapper.Update(fullBestelling);

            _eventPublisher.Publish(new BestellingToegevoegdEvent {Bestelling = bestelling});
            _eventPublisher.Publish(new KlantKredietAangepastEvent() { KlantId = fullBestelling.Klant.Id, NieuweKrediet = fullBestelling.Klant.KredietOver });

            _logger.LogInformation("Created new Bestelling with id {id} and {amount} artikelen", bestelling.Id,
                bestelling.BesteldeArtikelen.Count);

            return bestelling.Id;
        }

        [Command(NameConstants.VolgendeBestellingCommandQueue)]
        public async Task<int> VolgendeBestellingInpakken(VolgendeBestellingCommand command)
        {
            var result = await _datamapper.GetFirstUndone();
            _logger.LogInformation("Volgende bestelling {0}", result);

            if (result != 0)
            {
                _eventPublisher.Publish(new BestellingInpakkenGestartEvent {Id = result});
                _logger.LogInformation("Next bestelling ({id})", result);
            }


            return result;
        }
        
        [Command(NameConstants.FinishBestellingCommandQueue)]
        public async Task<int> FinishBestelling(BestellingAfrondenCommand command)
        {
            var bestelling = await _datamapper.GetBestelling(command.Id);

            foreach (var bestellingItem in bestelling.BesteldeArtikelen)
                try
                {
                    _logger.LogInformation("Lowering voorraad by {0} of artikel {1}", bestellingItem.Aantal,
                        bestellingItem.Artikelnummer);
                    var result = await _commandPublisher.Publish<bool>(
                        new HaalVoorraadUitMagazijnCommand(bestellingItem.Artikelnummer, bestellingItem.Aantal),
                        "Kantilever.MagazijnService", "Kantilever.MagazijnService.HaalVoorraadUitMagazijnCommand");
                }
                catch (FunctionalException e)
                {
                    //Do nothing yet because not implemented.
                    _logger.LogWarning(
                        "Tried finishing a bestelling with not enough voorraad, this should not happen. {0}",
                        e.Message);
                }

            _eventPublisher.Publish(new BestellingVerzondenEvent {Id = bestelling.Id});

            _logger.LogInformation("Finished bestelling {0}", bestelling.Id);
            return command.Id;
        }

        [Command(NameConstants.BestellingGoedKeurenCommandQueue)]
        public async Task<int> KeurBestellingGoed(BestellingGoedkeurenCommand bestellingGoedkeurenCommand)
        {
            var bestellingToBeUpdated = await _datamapper.GetBestelling(bestellingGoedkeurenCommand.Id);

            bestellingToBeUpdated.BestellingStatus = BestellingStatus.GereedVoorBehandeling;
            await _datamapper.Update(bestellingToBeUpdated);

            bestellingToBeUpdated.Klant.KredietOver -= bestellingToBeUpdated.GetTotaalPrijs;
            await _klantDatamapper.Update(bestellingToBeUpdated.Klant);


            _eventPublisher.Publish(new BestellingGoedGekeurdEvent {Id = bestellingToBeUpdated.Id});
            _eventPublisher.Publish(new KlantKredietAangepastEvent() { KlantId = bestellingToBeUpdated.Klant.Id, NieuweKrediet = bestellingToBeUpdated.Klant.KredietOver });

            _logger.LogInformation("Accepted bestelling {id} by sales", bestellingToBeUpdated.Id);


            return bestellingToBeUpdated.Id;
        }

        [Command(NameConstants.BestellingAfkeurenCommandQueue)]
        public async Task<BestellingAfkeurenResult> KeurBestellingAf(BestellingAfkeurenCommand bestellingAfkeurenCommand)
        {
            var bestellingToBeUpdated = await _datamapper.GetBestelling(bestellingAfkeurenCommand.Id);

            bestellingToBeUpdated.Klant.KredietOver += bestellingToBeUpdated.GetTotaalPrijs;
            bestellingToBeUpdated.Klant.KredietMetSales -= bestellingToBeUpdated.GetTotaalPrijs;

            List<int> bestellingen = await RefreshKlantBestellingen(bestellingToBeUpdated.Klant);

            await _klantDatamapper.Update(bestellingToBeUpdated.Klant);

            bestellingToBeUpdated.BestellingStatus = BestellingStatus.Afgekeurd;
            await _datamapper.Update(bestellingToBeUpdated);

            _eventPublisher.Publish(new BestellingAfgekeurdEvent {Id = bestellingToBeUpdated.Id});
            _eventPublisher.Publish(new KlantKredietAangepastEvent() {KlantId = bestellingToBeUpdated.Klant.Id, NieuweKrediet = bestellingToBeUpdated.Klant.KredietOver });

            var bestellingAfkeuren = new BestellingAfkeurenResult()
            {
                Id = bestellingToBeUpdated.Id,
                IsSuccesfull = true,
                PassedBestellingen = bestellingen
            };

            return bestellingAfkeuren;
        }

        [Command(NameConstants.BetaalBestellingCommandQueue)]
        public async Task<int> BetaalBestelling(BetaalBestellingCommand bestellingCommand)
        {
            var klant = await _klantDatamapper.GetKlantWithBestellingId(bestellingCommand.BestellingId);

            if (klant == null) return 0;

            klant.KredietOver += bestellingCommand.Bedrag;
            klant.KredietMetSales -= bestellingCommand.Bedrag;

            await RefreshKlantBestellingen(klant);
            await _klantDatamapper.Update(klant);

            _eventPublisher.Publish(new KlantKredietAangepastEvent() { KlantId = klant.Id, NieuweKrediet = klant.KredietOver });

            return bestellingCommand.BestellingId;
        }

        private async Task<List<int>> RefreshKlantBestellingen(Klant klant)
        {
            var bestellingen = await _klantDatamapper.GetUnFinishedBestellingenOfKlant(klant.Id);

            var bestellingenPassed = new List<int>();

            foreach (var bestelling in bestellingen)
            { 
                if (bestelling.GetTotaalPrijs + klant.KredietMetSales < klant.KredietOver)
                {
                    klant.KredietOver -= bestelling.GetTotaalPrijs;
                    bestelling.BestellingStatus = BestellingStatus.GereedVoorBehandeling;
                    await _datamapper.Update(bestelling);

                    bestellingenPassed.Add(bestelling.Id);
                    _eventPublisher.Publish(new BestellingGoedGekeurdEvent {Id = bestelling.Id});
                    _eventPublisher.Publish(new KlantKredietAangepastEvent() { KlantId = klant.Id, NieuweKrediet = klant.KredietOver });
                }
            }
            return bestellingenPassed;
        }   
    }
}