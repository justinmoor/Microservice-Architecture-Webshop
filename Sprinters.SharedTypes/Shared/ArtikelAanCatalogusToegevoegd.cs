using System;
using System.Collections.Generic;
using Minor.Nijn.WebScale.Events;

namespace Sprinters.SharedTypes.Shared
{
    public class ArtikelAanCatalogusToegevoegd : DomainEvent
    {
        public ArtikelAanCatalogusToegevoegd() :
            base("Kantilever.CatalogusService.ArtikelAanCatalogusToegevoegd")
        {
            Categorieen = new List<string>();
        }

        public int Artikelnummer { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public decimal Prijs { get; set; }
        public string AfbeeldingUrl { get; set; }
        public DateTime LeverbaarVanaf { get; set; }
        public DateTime? LeverbaarTot { get; set; }
        public string Leveranciercode { get; set; }
        public string Leverancier { get; set; }
        public List<string> Categorieen { get; set; }
    }
}