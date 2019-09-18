namespace Sprinters.SharedTypes.BeheerService.Entities
{
    public class WinkelmandItem
    {
        public WinkelmandItem()
        {
        }

        public WinkelmandItem(int artikelNummer, int aantal)
        {
            ArtikelNummer = artikelNummer;
            Aantal = aantal;
        }

        public int ArtikelNummer { get; set; }
        public int Aantal { get; set; }
    }
}