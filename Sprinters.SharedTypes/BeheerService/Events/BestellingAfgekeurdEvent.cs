using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.BeheerService.Events
{
    public class BestellingAfgekeurdEvent : DomainEvent
    {
        public BestellingAfgekeurdEvent() : base(NameConstants.BestellingAfgekeurdEvent)
        {
        }

        public int Id { get; set; }
    }
}