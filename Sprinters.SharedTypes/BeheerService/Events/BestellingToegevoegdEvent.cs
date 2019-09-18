using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.BeheerService.Events
{
    public class BestellingToegevoegdEvent : DomainEvent
    {
        public BestellingToegevoegdEvent() : base(NameConstants.BestellingToegevoegdEvent)
        {
        }

        public Bestelling Bestelling { get; set; }
    }
}