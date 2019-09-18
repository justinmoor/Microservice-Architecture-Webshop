using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprinters.Webshop.BFF.Entities
{
    public class BestellingItem
    {
        public BestellingItem(int artikelnummer, int aantal)
        {
            Aantal = aantal;
            Artikelnummer = artikelnummer;
        }

        public BestellingItem()
        {
        }

        [Key] public int Id { get; set; }

        public int Aantal { get; set; }

        public Artikel Artikel { get; set; }

        [ForeignKey("ArtikelId")] public int Artikelnummer { get; set; }
    }
}