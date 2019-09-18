using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;
using Sprinters.Webshop.BFF.Listeners;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class MagazijnListenerTest
    {
        [TestMethod]
        public void ArtikelToegevoegdInsertsNewArtikel()
        {
            var artikelEvent = new ArtikelAanCatalogusToegevoegd
            {
                AfbeeldingUrl = "Afbeelding.jpg",
                Artikelnummer = 1234,
                Beschrijving = "Grote fiets voor iedereen",
                Leverancier = "Fietsen bv",
                Leveranciercode = "1",
                LeverbaarTot = new DateTime(2018, 5, 5),
                LeverbaarVanaf = new DateTime(2017, 1, 1),
                Naam = "Fiets",
                Prijs = 299.3m
            };

            var mapperMock = new Mock<IArtikelDatamapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.Insert(It.Is<Artikel>(a => a.Artikelnummer == 1234))).Verifiable();
            var magazijnListener = new MagazijnListener(mapperMock.Object);

            magazijnListener.ArtikelToegevoegdEvent(artikelEvent);
        }

        [TestMethod]
        public void VoorraadVerhoogEventVerhoogdVoorraad()
        {
            var voorraadEvent = new VoorraadVerhoogdEvent
            {
                Artikelnummer = 1234,
                Aantal = 5,
                NieuweVoorraad = 5
            };

            var mapperMock = new Mock<IArtikelDatamapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.ChangeVoorraad(1234, 5)).Verifiable();
            var magazijnListener = new MagazijnListener(mapperMock.Object);

            magazijnListener.VoorraadVerhoogdEvent(voorraadEvent);
        }

        [TestMethod]
        public void VoorraadVerlaagEventVerlaagdVoorraad()
        {
            var voorraadEvent = new VoorraadVerlaagdEvent
            {
                Artikelnummer = 1234,
                Aantal = 5,
                NieuweVoorraad = 0
            };

            var mapperMock = new Mock<IArtikelDatamapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.ChangeVoorraad(1234, 0)).Verifiable();
            var magazijnListener = new MagazijnListener(mapperMock.Object);

            magazijnListener.VoorraadVerlaagdEvent(voorraadEvent);
        }
    }
}