using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.BeheerService.Events
{
    public class BestellingGoedGekeurdEvent : DomainEvent
    {
        public BestellingGoedGekeurdEvent() : base(NameConstants.BestellingGoedgekeurdEvent)
        {
        }

        public int Id { get; set; }
    }
}