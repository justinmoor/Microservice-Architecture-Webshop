using System;
using System.ComponentModel.DataAnnotations;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.Webshop.BFF.Entities
{
    public class Artikel
    {
        [Key] public int Artikelnummer { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }

        public decimal Prijs { get; set;}
    
        public decimal PrijsWithBtw
        {
            get => Math.Round(Prijs * NameConstants.BtwTarief, 2, MidpointRounding.ToEven);
            set => Prijs = value;
        }

        public string AfbeeldingUrl { get; set; }
        public DateTime LeverbaarVanaf { get; set; }
        public DateTime? LeverbaarTot { get; set; }
        public string Leveranciercode { get; set; }
        public string Leverancier { get; set; }
        public int Voorraad { get; set; }
    }
}