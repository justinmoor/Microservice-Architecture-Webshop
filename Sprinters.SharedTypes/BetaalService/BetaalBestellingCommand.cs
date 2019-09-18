using System;
using System.Collections.Generic;
using System.Text;
using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.BetaalService
{
    public class BetaalBestellingCommand : DomainCommand
    { 
        public int BestellingId { get; set; }
        public decimal Bedrag { get; set; }

    }
}
