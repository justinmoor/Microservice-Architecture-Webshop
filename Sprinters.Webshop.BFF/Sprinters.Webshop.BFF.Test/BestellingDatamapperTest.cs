using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.Webshop.BFF.DAL;
using Artikel = Sprinters.Webshop.BFF.Entities.Artikel;
using Bestelling = Sprinters.Webshop.BFF.Entities.Bestelling;
using BestellingItem = Sprinters.Webshop.BFF.Entities.BestellingItem;
using Klant = Sprinters.Webshop.BFF.Entities.Klant;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class BestellingDatamapperTest
    {
        private SqliteConnection _connection;
        private DbContextOptions<WebshopContext> _options;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<WebshopContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new WebshopContext(_options))
            {
                context.Database.EnsureCreated();
            }

            var artikel = new Artikel
            {
                Artikelnummer = 1,
                Beschrijving = "Grote fiets voor iedereen",
                LeverbaarTot = new DateTime(2018, 5, 5),
                LeverbaarVanaf = new DateTime(2017, 1, 1),
                Naam = "Fiets",
                Prijs = 100.50m,
                Voorraad = 5
            };

            var artikel2 = new Artikel
            {
                Artikelnummer = 2,
                Beschrijving = "HELE grote fiets voor iedereen",
                LeverbaarTot = new DateTime(2018, 5, 5),
                LeverbaarVanaf = new DateTime(2017, 1, 1),
                Naam = "Fiets Groot",
                Prijs = 600.50m,
                Voorraad = 8
            };

            var klant = new Klant
            {
                Voornaam = "Hans",
                Achternaam = "Van Huizen",
                Id = "1",
                Telefoonnummer = "0612341234",
                AdresRegel = "Grote straat 1",
                Email = "Hans@vanHuizen.nl",
                Plaats = "Groningen",
                Postcode = "6123AA",
                Wachtwoord = "Geheim_101"
            };


            using (var context = new WebshopContext(_options))
            {
                var klantDatamapper = new KlantDatamapper(context);
                klantDatamapper.Insert(klant);

                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
                dataMapper.Insert(artikel2);
            }
        }

        [TestMethod]
        public async Task InsertBestellingIntoDatabaseAndUpdate()
        {
            var bestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Id = 1},
                    new BestellingItem(2, 5) {Id = 2}
                }
            };
            using (var context = new WebshopContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                await mapper.Insert(bestelling);
            }

            using (var context = new WebshopContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.Get(1);

                Assert.AreEqual("1", result.KlantId);
                Assert.AreEqual("Laagstraat 11", result.AdresRegel1);
                Assert.AreEqual("Laaghoven", result.Plaats);
                Assert.AreEqual("1234FG", result.Postcode);
                Assert.AreEqual(2, result.BesteldeArtikelen.Count);
                Assert.IsTrue(result.BesteldeArtikelen.Any(b => b.Artikel.Naam == "Fiets" && b.Aantal == 3));

                result.BestellingStatus = BestellingStatus.GereedVoorBehandeling;
                using (var context2 = new WebshopContext(_options))
                {
                    var mapper2 = new BestellingDatamapper(context2);
                    await mapper2.Update(result);
                }
            }

            using (var context = new WebshopContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.Get(1);

                Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, result.BestellingStatus);
            }
        }

        [TestMethod]
        public async Task GetBestellingenAbove500EurAndNotAcceptedBySalesOnlyReturnsThose()
        {
            var bestellingBoven500 = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(2, 1) {Id = 1}
                }
            };

            var bestellingOnder500 = new Bestelling
            {
                Id = 2,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.GereedVoorBehandeling,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Id = 2}
                }
            };

            var bestellingBoven500GereedVoorBehandeling = new Bestelling
            {
                Id = 3,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.GereedVoorBehandeling,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(2, 1) {Id = 3}
                }
                 
            };

            using (var context = new WebshopContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                await mapper.Insert(bestellingBoven500);
                await mapper.Insert(bestellingOnder500);
                await mapper.Insert(bestellingBoven500GereedVoorBehandeling);
            }

            using (var context = new WebshopContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.GetBestellingenBoven500() as List<Bestelling>;

                Assert.IsTrue(result.Any(b => b.Id == bestellingBoven500.Id));
                Assert.IsFalse(result.Any(b => b.Id == bestellingOnder500.Id));
                Assert.IsFalse(result.Any(b => b.Id == bestellingBoven500GereedVoorBehandeling.Id));
            }
        }
    }
}