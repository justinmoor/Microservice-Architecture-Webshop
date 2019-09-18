using Minor.Nijn.WebScale.Events;

namespace Sprinters.SharedTypes.Shared
{
    public class VoorraadVerhoogdEvent : DomainEvent
    {
        public VoorraadVerhoogdEvent() : base("Kantilever.MagazijnService.VoorraadVerhoogdEvent")
        {
        }

        public int Artikelnummer { get; set; }
        public int Aantal { get; set; }
        public int NieuweVoorraad { get; set; }
    }
}