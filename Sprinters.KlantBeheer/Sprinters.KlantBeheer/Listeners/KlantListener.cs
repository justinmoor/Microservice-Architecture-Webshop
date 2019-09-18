using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Attributes;
using Sprinters.KlantBeheer.DAL;
using Sprinters.SharedTypes.KlantService.Commands;
using Sprinters.SharedTypes.KlantService.Entities;
using Sprinters.SharedTypes.KlantService.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.KlantBeheer.Listeners
{
    [CommandListener]
    public class KlantListener
    {
        private readonly IKlantDatamapper _datamapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public KlantListener(IKlantDatamapper datamapper, IEventPublisher eventPublisher)
        {
            _datamapper = datamapper;
            _eventPublisher = eventPublisher;
            _logger = NijnLogger.CreateLogger<KlantListener>();
        }

        [Command(NameConstants.RegistreerKlantCommandQueue)]
        public async Task<string> RegistreerKlant(RegistreerKlantCommand command)
        {
            var klant = new Klant
            {
                Achternaam = command.Achternaam,
                Voornaam = command.Voornaam,
                AdresRegel = command.AdresRegel,
                Id = command.AccountId,
                Plaats = command.Plaats,
                Postcode = command.Postcode,
                Telefoonnummer = command.Telefoonnummer
            };

            await _datamapper.Insert(klant);

            _logger.LogInformation("Added klant with the id {0} and name {1} {2} ", klant.Id, klant.Voornaam,
                klant.Achternaam);

            _eventPublisher.Publish(new KlantGeregistreerdEvent
            {
                Achternaam = klant.Achternaam,
                Voornaam = klant.Voornaam,
                AdresRegel = klant.AdresRegel,
                Id = klant.Id,
                Plaats = klant.Plaats,
                Postcode = klant.Postcode,
                Telefoonnummer = klant.Telefoonnummer
            });

            return klant.Id;
        }
    }
}