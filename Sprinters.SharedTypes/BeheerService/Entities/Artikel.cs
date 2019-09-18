using System;
using System.ComponentModel.DataAnnotations;

namespace Sprinters.SharedTypes.BeheerService.Entities
{
    public class Artikel
    {
        [Key] public int Artikelnummer { get; set; }

        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public decimal Prijs { get; set; }
        public DateTime LeverbaarVanaf { get; set; }
        public DateTime? LeverbaarTot { get; set; }

        public int Voorraad { get; set; }
    }
}