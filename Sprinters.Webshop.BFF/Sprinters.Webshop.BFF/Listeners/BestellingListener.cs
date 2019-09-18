using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale.Attributes;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.BeheerService.Events;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using BestellingToegevoegdEvent = Sprinters.Webshop.BFF.Entities.BestellingToegevoegdEvent;

namespace Sprinters.Webshop.BFF.Listeners
{
    [EventListener("WebshopEventListener")]
    public class BestellingListener
    {
        private readonly IBestellingDatamapper _datamapper;
        private readonly ILogger _logger;


        public BestellingListener(IBestellingDatamapper datamapper)
        {
            _datamapper = datamapper;
            _logger = NijnLogger.CreateLogger<BestellingListener>();
        }


        [Topic(NameConstants.BestellingToegevoegdEvent)]
        public void BestellingToegevoegd(BestellingToegevoegdEvent bestellingEvent)
        {
            bestellingEvent.Bestelling.BesteldeArtikelen.ForEach(b => b.Artikel = null);
            bestellingEvent.Bestelling.Klant = null;

            _datamapper.Insert(bestellingEvent.Bestelling);
            _logger.LogDebug("Added bestelling {id}", bestellingEvent.Bestelling.Id);
        }

        [Topic(NameConstants.BestellingVerzondenEvent)]
        public async Task BestellingVerzonden(BestellingVerzondenEvent bestellingVerzondenEvent)
        {
            var bestelling = await _datamapper.Get(bestellingVerzondenEvent.Id);
            bestelling.BestellingStatus = BestellingStatus.Verzonden;

            await _datamapper.Update(bestelling);
            _logger.LogDebug("Updated bestelling {id} to verzonden", bestellingVerzondenEvent.Id);
        }

        [Topic(NameConstants.BestellingInpakkenGestartEvent)]
        public async Task BestellingInpakkenGestart(BestellingInpakkenGestartEvent bestellingInpakkenGestartEvent)
        {
            var bestelling = await _datamapper.Get(bestellingInpakkenGestartEvent.Id);
            bestelling.BestellingStatus = BestellingStatus.InBehandelingDoorMagazijn;

            await _datamapper.Update(bestelling);
            _logger.LogDebug("Updated bestelling {id} to inpakken gestart", bestellingInpakkenGestartEvent.Id);
        }

        [Topic(NameConstants.BestellingGoedgekeurdEvent)]
        public async Task BestellingGoedgekeurd(BestellingGoedGekeurdEvent bestellingGoedGekeurdEvent)
        {
            var bestelling = await _datamapper.Get(bestellingGoedGekeurdEvent.Id);
            bestelling.BestellingStatus = BestellingStatus.GereedVoorBehandeling;

            await _datamapper.Update(bestelling);
            _logger.LogDebug("Updated bestelling {id} to goedgekeurd", bestellingGoedGekeurdEvent.Id);
        }

        [Topic(NameConstants.BestellingAfgekeurdEvent)]
        public async Task BestellingAfgekeurd(BestellingAfgekeurdEvent bestellingGoedGekeurdEvent)
        {
            var bestelling = await _datamapper.Get(bestellingGoedGekeurdEvent.Id);
            bestelling.BestellingStatus = BestellingStatus.Afgekeurd;

            await _datamapper.Update(bestelling);
        }
    }
}