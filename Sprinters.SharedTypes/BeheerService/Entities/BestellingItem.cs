using System.ComponentModel.DataAnnotations.Schema;

namespace Sprinters.SharedTypes.BeheerService.Entities
{
    public class BestellingItem
    {
        public BestellingItem()
        {
        }

        public BestellingItem(int artikelId, int aantal)
        {
            Artikelnummer = artikelId;
            Aantal = aantal;
        }

        public int Id { get; set; }
        public Artikel Artikel { get; set; }

        [ForeignKey("ArtikelId")] public int Artikelnummer { get; set; }

        public int Aantal { get; set; }
    }
}