using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class ArtikelDatamapperTest
    {
        private SqliteConnection connection;
        private DbContextOptions<WebshopContext> options;

        [TestInitialize]
        public void Initialize()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            options = new DbContextOptionsBuilder<WebshopContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new WebshopContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestMethod]
        public async Task InsertAddsArtikelToDatabase()
        {
            var artikel = new Artikel
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

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
            }

            using (var context = new WebshopContext(options))
            {
                var artikelDatamapper = new ArtikelDatamapper(context);
                var result = await artikelDatamapper.Get(1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(0, result.Voorraad);
            }
        }


        [TestMethod]
        public void ChangeVoorraadChangesVoorraadOfArtikel()
        {
            var artikel = new Artikel
            {
                AfbeeldingUrl = "Afbeelding.jpg",
                Artikelnummer = 1234,
                Beschrijving = "Grote fiets voor iedereen",
                Leverancier = "Fietsen bv",
                Leveranciercode = "1",
                LeverbaarTot = new DateTime(2019, 8, 8),
                LeverbaarVanaf = new DateTime(2017, 3, 3),
                Naam = "Fiets",
                Prijs = 299.3m
            };

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
            }

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.ChangeVoorraad(1234, 10);
            }

            using (var context = new WebshopContext(options))
            {
                var result = context.Artikelen.SingleOrDefault(a => a.Artikelnummer == 1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(10, result.Voorraad);
            }
        }

        [TestMethod]
        public async Task GetAllArtikelenReturnsAll()
        {
            var artikel = new Artikel
            {
                
                AfbeeldingUrl = "Afbeelding.jpg",
                Artikelnummer = 1,
                Beschrijving = "Grote fiets voor iedereen",
                Leverancier = "Fietsen bv",
                Leveranciercode = "1",
                LeverbaarTot = new DateTime(2019, 8, 8),
                LeverbaarVanaf = new DateTime(2017, 3, 3),
                Naam = "Fiets",
                Prijs = 299.3m
            };

            var artikel2 = new Artikel
            {
                AfbeeldingUrl = "Afbeelding.jpg",
                Artikelnummer = 2,
                Beschrijving = "Ultra zacht zadel voor de meest comfortabele fietstocht",
                Leverancier = "FietszadelsWinkel",
                Leveranciercode = "3",
                LeverbaarTot = new DateTime(2019, 8, 8),
                LeverbaarVanaf = new DateTime(2017, 3, 3),
                Naam = "Zadel",
                Prijs = 19.49m
            };

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
                dataMapper.Insert(artikel2);
            }
            using (var context = new WebshopContext(options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                var artikelen = await dataMapper.GetAll();

                Assert.AreEqual(2, artikelen.Count);
                Assert.IsTrue(artikelen.Any(a => a.Artikelnummer == 2 && a.Naam == "Zadel" && a.PrijsWithBtw == 23.58m));
                Assert.IsTrue(artikelen.Any(a => a.Artikelnummer == 1 && a.Naam == "Fiets" && a.PrijsWithBtw == 362.15m));
            }
        }
    }
}