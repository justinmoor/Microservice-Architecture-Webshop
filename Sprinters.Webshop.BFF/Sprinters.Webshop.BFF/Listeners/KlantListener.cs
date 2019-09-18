using System.Threading.Tasks;
using Minor.Nijn.WebScale.Attributes;
using Sprinters.SharedTypes.BetaalService;
using Sprinters.SharedTypes.KlantService.Events;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Listeners
{
    [EventListener("WebshopEventListener")]
    public class KlantListener
    {
        private readonly IKlantDatamapper _klantDatamapper;

        public KlantListener(IKlantDatamapper klantDatamapper)
        {
            _klantDatamapper = klantDatamapper;
        }

        [Topic(NameConstants.KlantGeregistreerdEvent)]
        public void KlantToegevoegd(KlantGeregistreerdEvent klantGeregistreerd)
        {
            var klant = new Klant
            {
                Id = klantGeregistreerd.Id,
                Voornaam = klantGeregistreerd.Voornaam,
                Achternaam = klantGeregistreerd.Achternaam,
                AdresRegel = klantGeregistreerd.AdresRegel,
                Plaats = klantGeregistreerd.Plaats,
                Postcode = klantGeregistreerd.Postcode,
                Telefoonnummer = klantGeregistreerd.Telefoonnummer
            };

            _klantDatamapper.Insert(klant);
        }

        [Topic(NameConstants.KlantKredietAangepastEvent)]
        public async Task KlantKredietAangepast(KlantKredietAangepastEvent klantKredietAangepast)
        {
            var klant = await _klantDatamapper.GetKlant(klantKredietAangepast.KlantId);
            klant.Krediet = klantKredietAangepast.NieuweKrediet;
            _klantDatamapper.Update(klant);
        }
    }
}