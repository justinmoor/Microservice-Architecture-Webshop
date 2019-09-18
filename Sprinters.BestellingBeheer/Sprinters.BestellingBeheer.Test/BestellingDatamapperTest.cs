using System;
using System.Collections.Generic;
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
    public class BestellingDatamapperTest
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
            var klant = new Klant {Voornaam = "Hans", Achternaam = "Van Huizen", Id = "1"};

            using (var context = new BeheerContext(_options))
            {
                var klantDatamapper = new KlantDatamapper(context);
                klantDatamapper.Insert(klant);

                var dataMapper = new ArtikelDatamapper(context);
                dataMapper.Insert(artikel);
                dataMapper.Insert(artikel2);
            }
        }

        [TestMethod]
        public async Task InsertBestellingIntoDatabase()
        {
            var bestelling = new Bestelling
            {
                BestellingStatus = BestellingStatus.GereedVoorBehandeling,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 5)
                }
            };
            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                await mapper.Insert(bestelling);
            }

            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.GetBestelling(1);

                Assert.AreEqual("Hans", result.Klant.Voornaam);
                Assert.AreEqual("Van Huizen", result.Klant.Achternaam);
                Assert.AreEqual("1", result.KlantId);
                Assert.AreEqual("Laagstraat 11", result.AdresRegel1);
                Assert.AreEqual("Laaghoven", result.Plaats);
                Assert.AreEqual("1234FG", result.Postcode);
                Assert.AreEqual(2, result.BesteldeArtikelen.Count);
                Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, result.BestellingStatus);
                Assert.IsTrue(result.BesteldeArtikelen.Any(b => b.Artikel.Naam == "Fiets" && b.Aantal == 3));
            }
        }

        [TestMethod]
        public async Task UpdateBestellingUpdatesIntoDatabase()
        {
            var bestelling = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 5)
                }
            };

            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                await mapper.Insert(bestelling);
            }

            bestelling.BestellingStatus = BestellingStatus.GereedVoorBehandeling;


            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                await mapper.Update(bestelling);
            }


            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.GetBestelling(1);

                Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, result.BestellingStatus);
            }
        }


        [TestMethod]
        public async Task GetVolgendeBestellingGivesNextBestelling()
        {
            var bestelling = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.GereedVoorBehandeling
            };
            var bestelling2Finished = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Middenland 34",
                Plaats = "Middenhoven",
                Postcode = "4535FF",
                BestellingStatus = BestellingStatus.Verzonden
            };
            var bestelling3 = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Hoogstraat 27",
                Plaats = "Hooghoven",
                Postcode = "4321PD",
                BestellingStatus = BestellingStatus.GereedVoorBehandeling
            };

            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                await mapper.Insert(bestelling);
                await mapper.Insert(bestelling2Finished);
                await mapper.Insert(bestelling3);
            }

            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.GetFirstUndone();
                Assert.AreEqual(1, result);

                //Should skip the second one
                var result2 = await mapper.GetFirstUndone();
                Assert.AreEqual(3, result2);
            }
        }

        [TestMethod]
        public async Task GetFirstUndoneReturnsNullWhenNoBestelling()
        {
            using (var context = new BeheerContext(_options))
            {
                var mapper = new BestellingDatamapper(context);
                var result = await mapper.GetFirstUndone();
                Assert.AreEqual(0, result);
            }
        }
    }
}