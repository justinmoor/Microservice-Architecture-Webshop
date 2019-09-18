using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale.Attributes;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.BestellingBeheer.Listeners
{
    [EventListener("BestellingBeheer")]
    public class MagazijnListener
    {
        private readonly IArtikelDatamapper _datamapper;
        private readonly ILogger _logger;

        public MagazijnListener(IArtikelDatamapper datamapper)
        {
            _datamapper = datamapper;
            _logger = NijnLogger.CreateLogger<MagazijnListener>();
        }

        [Topic("Kantilever.CatalogusService.ArtikelAanCatalogusToegevoegd")]
        public void ArtikelToegevoegdEvent(ArtikelAanCatalogusToegevoegd artikelAanCatalogusToegevoegd)
        {
            var artikel = new Artikel
            {
                Artikelnummer = artikelAanCatalogusToegevoegd.Artikelnummer,
                Beschrijving = artikelAanCatalogusToegevoegd.Beschrijving,
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
            _logger.LogDebug("Updated artikelen (id {id}) voorraad verlaagd to {amount}",
                voorraadVerlaagdEvent.Artikelnummer, voorraadVerlaagdEvent.NieuweVoorraad);
        }

        [Topic("Kantilever.MagazijnService.VoorraadVerhoogdEvent")]
        public void VoorraadVerhoogdEvent(VoorraadVerhoogdEvent voorraadVerhoogdEvent)
        {
            _datamapper.ChangeVoorraad(voorraadVerhoogdEvent.Artikelnummer, voorraadVerhoogdEvent.NieuweVoorraad);
            _logger.LogDebug("Updated artikelen (id {id}) voorraad verhoogd to {amount} ",
                voorraadVerhoogdEvent.Artikelnummer, voorraadVerhoogdEvent.NieuweVoorraad);
        }
    }
}