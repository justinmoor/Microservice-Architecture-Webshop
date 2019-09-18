using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprinters.SharedTypes.BeheerService.Entities
{
    public class Bestelling
    {
        public int Id { get; set; }


        public string AdresRegel1 { get; set; }
        public string AdresRegel2 { get; set; }
        public string Plaats { get; set; }
        public string Postcode { get; set; }

        public List<BestellingItem> BesteldeArtikelen { get; set; }

        public BestellingStatus BestellingStatus { get; set; }

        public Klant Klant { get; set; }
        public string KlantId { get; set; }

        public decimal GetTotaalPrijs => BesteldeArtikelen.Sum(item =>
            item.Aantal * Math.Round(item.Artikel.Prijs * 1.21m, 2, MidpointRounding.ToEven));
    }
}