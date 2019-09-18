using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.Shared
{
    public class HaalVoorraadUitMagazijnCommand : DomainCommand
    {
        public HaalVoorraadUitMagazijnCommand(int artikelnummer, int aantal)
        {
            Artikelnummer = artikelnummer;
            Aantal = aantal;
        }

        public int Artikelnummer { get; set; }
        public int Aantal { get; set; }
    }
}