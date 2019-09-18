using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.Webshop.BFF.Entities
{
    public class BestellingToegevoegdEvent : DomainEvent
    {
        public BestellingToegevoegdEvent() : base(NameConstants.BestellingToegevoegdEvent)
        {
        }

        public Bestelling Bestelling { get; set; }
    }
}