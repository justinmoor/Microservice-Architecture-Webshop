using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.Test
{
    [TestClass]
    public class ArtikelDatamapperTest
    {
        private SqliteConnection _connection;
        private DbContextOptions<BeheerContext> _options;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BeheerContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new BeheerContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestMethod]
        public void InsertAddsArtikelToDatabase()
        {
            var artikel = new Artikel
            {
                Artikelnummer = 1234,
                Beschrijving = "Grote fiets voor iedereen",
                LeverbaarTot = new DateTime(2018, 5, 5),
                LeverbaarVanaf = new DateTime(2017, 1, 1),
                Naam = "Fiets",
                Prijs = 299.3m
            };

            using (var context = new BeheerContext(_options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
            }

            using (var context = new BeheerContext(_options))
            {
                var result = context.Artikelen.SingleOrDefault(a => a.Artikelnummer == 1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(0, result.Voorraad);
            }
        }


        [TestMethod]
        public async Task getAllArtikelenReturnsAll()
        {
            var artikel = new Artikel
            {
                Artikelnummer = 1,
                Beschrijving = "Grote fiets voor iedereen",
                LeverbaarTot = new DateTime(2018, 5, 5),
                LeverbaarVanaf = new DateTime(2017, 1, 1),
                Naam = "Fiets",
                Prijs = 299.3m,
                Voorraad = 5
            };
            var artikel2 = new Artikel
            {
                Artikelnummer = 2,
                Beschrijving = "HELE grote fiets voor iedereen",
                LeverbaarTot = new DateTime(2018, 5, 5),
                LeverbaarVanaf = new DateTime(2017, 1, 1),
                Naam = "Fiets Groot",
                Prijs = 299.3m,
                Voorraad = 8
            };

            using (var context = new BeheerContext(_options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
                dataMapper.Insert(artikel2);
            }

            using (var context = new BeheerContext(_options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                var all = await dataMapper.GetAll();
                Assert.AreEqual(2, all.Count);
                Assert.IsTrue(all.Any(a => a.Naam == "Fiets Groot"));
            }
        }

        [TestMethod]
        public void ChangeVoorraadChangesVoorraadOfArtikel()
        {
            var artikel = new Artikel
            {
                Artikelnummer = 1234,
                Beschrijving = "Grote fiets voor iedereen",
                LeverbaarTot = new DateTime(2019, 8, 8),
                LeverbaarVanaf = new DateTime(2017, 3, 3),
                Naam = "Fiets",
                Prijs = 299.3m
            };

            using (var context = new BeheerContext(_options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
            }

            using (var context = new BeheerContext(_options))
            {
                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.ChangeVoorraad(1234, 10);
            }

            using (var context = new BeheerContext(_options))
            {
                var result = context.Artikelen.SingleOrDefault(a => a.Artikelnummer == 1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(10, result.Voorraad);
            }
        }
    }
}