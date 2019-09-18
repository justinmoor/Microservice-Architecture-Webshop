using System;
using System.Collections.Generic;
using System.Text;
using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.SharedTypes.BetaalService
{
    public class KlantKredietAangepastEvent : DomainEvent
    {
        public KlantKredietAangepastEvent() : base(NameConstants.KlantKredietAangepastEvent)
        {
        }

        public string KlantId { get; set; }

        public decimal NieuweKrediet { get; set; }

    }
}
