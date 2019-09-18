using Minor.Nijn.WebScale.Attributes;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.KlantService.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.BestellingBeheer.Listeners
{
    [EventListener("BestellingBeheer")]
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
                KredietOver = NameConstants.KredietLimit,
            };

            _klantDatamapper.Insert(klant);
        }
    }
}