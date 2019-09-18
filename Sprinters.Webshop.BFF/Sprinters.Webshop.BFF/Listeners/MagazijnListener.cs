using Minor.Nijn.WebScale.Attributes;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Listeners
{
    [EventListener("WebshopEventListener")]
    public class MagazijnListener
    {
        private readonly IArtikelDatamapper _datamapper;

        public MagazijnListener(IArtikelDatamapper datamapper)
        {
            _datamapper = datamapper;
        }

        [Topic("Kantilever.CatalogusService.ArtikelAanCatalogusToegevoegd")]
        public void ArtikelToegevoegdEvent(ArtikelAanCatalogusToegevoegd artikelAanCatalogusToegevoegd)
        {
            var artikel = new Artikel
            {
                AfbeeldingUrl = artikelAanCatalogusToegevoegd.AfbeeldingUrl,
                Artikelnummer = artikelAanCatalogusToegevoegd.Artikelnummer,
                Beschrijving = artikelAanCatalogusToegevoegd.Beschrijving,
                Leverancier = artikelAanCatalogusToegevoegd.Leverancier,
                Leveranciercode = artikelAanCatalogusToegevoegd.Leveranciercode,
                LeverbaarTot = artikelAanCatalogusToegevoegd.LeverbaarTot,
                LeverbaarVanaf = artikelAanCatalogusToegevoegd.LeverbaarVanaf,
                Naam = artikelAanCatalogusToegevoegd.Naam,
                Prijs = artikelAanCatalogusToegevoegd.Prijs
            };

            _datamapper.Insert(artikel);
        }

        [Topic("Kantilever.MagazijnService.VoorraadVerlaagdEvent")]
        public void VoorraadVerlaagdEvent(VoorraadVerlaagdEvent voorraadVerlaagdEvent)
        {
            _datamapper.ChangeVoorraad(voorraadVerlaagdEvent.Artikelnummer, voorraadVerlaagdEvent.NieuweVoorraad);
        }

        [Topic("Kantilever.MagazijnService.VoorraadVerhoogdEvent")]
        public void VoorraadVerhoogdEvent(VoorraadVerhoogdEvent voorraadVerhoogdEvent)
        {
            _datamapper.ChangeVoorraad(voorraadVerhoogdEvent.Artikelnummer, voorraadVerhoogdEvent.NieuweVoorraad);
        }
    }
}