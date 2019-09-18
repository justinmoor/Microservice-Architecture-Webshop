using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sprinters.Webshop.BFF.Controllers;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class ArtikelenControllerTest
    {
        [TestMethod]
        public async Task GetArtikelenReturnsAllArtikelen()
        {
            var mapperMock = new Mock<IArtikelDatamapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.GetAll()).ReturnsAsync(
                new List<Artikel>
                {
                    new Artikel
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
                    },

                    new Artikel
                    {
                        AfbeeldingUrl = "Afbeelding2.jpg",
                        Artikelnummer = 4321,
                        Beschrijving = "Kleine fiets voor iedereen",
                        Leverancier = "Fietsen bv",
                        Leveranciercode = "1",
                        LeverbaarTot = new DateTime(2030, 5, 5),
                        LeverbaarVanaf = new DateTime(2017, 5, 5),
                        Naam = "Kleine Fiets",
                        Prijs = 199.3m
                    }
                }
            ).Verifiable();

            var controller = new ArtikelenController(mapperMock.Object);

            var artikelen = await controller.GetArtikelen();

            Assert.AreEqual(2, artikelen.Value.Count);
            Assert.IsTrue(artikelen.Value.Any(a => a.Artikelnummer == 4321));
            Assert.IsTrue(artikelen.Value.Any(a => a.Artikelnummer == 1234));
        }

        [TestMethod]
        public async Task GetArtikelReturnsArtikel()
        {
            var mapperMock = new Mock<IArtikelDatamapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.Get(1234)).ReturnsAsync(
                    new Artikel
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
                    }
                );

            var controller = new ArtikelenController(mapperMock.Object);

            var artikel = await controller.GetArtikel(1234);

            Assert.AreEqual(1234, artikel.Artikelnummer);
            Assert.AreEqual("Fiets", artikel.Naam);

        }
    }
}