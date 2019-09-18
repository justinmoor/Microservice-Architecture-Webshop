using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.BeheerService.Events
{
    public class BestellingVerzondenEvent : DomainEvent
    {
        public BestellingVerzondenEvent() : base(NameConstants.BestellingVerzondenEvent)
        {
        }

        public int Id { get; set; }
    }
}