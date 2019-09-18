using Minor.Nijn.WebScale.Events;

namespace Sprinters.SharedTypes.Shared
{
    public class VoorraadVerlaagdEvent : DomainEvent
    {
        public VoorraadVerlaagdEvent() : base("Kantilever.MagazijnService.VoorraadVerlaagdEvent")
        {
        }

        public int Artikelnummer { get; set; }
        public int Aantal { get; set; }
        public int NieuweVoorraad { get; set; }
    }
}