﻿using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.BeheerService.Commands
{
    public class BestellingAfrondenCommand : DomainCommand
    {
        public int Id { get; set; }
    }
}