using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.Shared
{
    public class PlaatsVoorraadInMagazijnCommand : DomainCommand
    {
        public int Artikelnummer { get; set; }
        public int Aantal { get; set; }
    }
}