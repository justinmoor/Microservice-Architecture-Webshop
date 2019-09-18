using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.BeheerService.Events
{
    public class BestellingInpakkenGestartEvent : DomainEvent
    {
        public BestellingInpakkenGestartEvent() : base(NameConstants.BestellingInpakkenGestartEvent)
        {
        }

        public int Id { get; set; }
    }
}