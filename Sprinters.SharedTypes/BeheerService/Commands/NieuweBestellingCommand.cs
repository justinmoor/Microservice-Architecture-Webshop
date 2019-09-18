using System.Collections.Generic;
using Minor.Nijn.WebScale.Commands;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.SharedTypes.BeheerService.Commands
{
    public class NieuweBestellingCommand : DomainCommand
    {
        public List<BestellingItem> BesteldeArtikelen;

        public string KlantId { get; set; }

        public string AdresRegel1 { get; set; }
        public string AdresRegel2 { get; set; }
        public string Plaats { get; set; }
        public string Postcode { get; set; }
    }
}