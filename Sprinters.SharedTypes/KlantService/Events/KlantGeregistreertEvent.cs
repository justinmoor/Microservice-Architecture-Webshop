using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.KlantService.Events
{
    public class KlantGeregistreerdEvent : DomainEvent
    {
        public KlantGeregistreerdEvent() : base(NameConstants.KlantGeregistreerdEvent)
        {
        }

        public string Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Telefoonnummer { get; set; }
        public string AdresRegel { get; set; }
        public string Plaats { get; set; }
        public string Postcode { get; set; }
    }
}