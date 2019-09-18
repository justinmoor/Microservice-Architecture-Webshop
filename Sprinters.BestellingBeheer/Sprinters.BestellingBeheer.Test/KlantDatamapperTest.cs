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
    public class KlantDatamapperTest
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
        public async Task InsertBestellingIntoDatabase()
        {
            var klant = new Klant {Voornaam = "Hans", Achternaam = "Van Huizen", Id = "1"};

            using (var context = new BeheerContext(_options))
            {
                var mapper = new KlantDatamapper(context);
                mapper.Insert(klant);
            }

            using (var context = new BeheerContext(_options))
            {
                var mapper = new KlantDatamapper(context);
                var result = await mapper.GetKlant("1");

                Assert.AreEqual("Hans", result.Voornaam);
                Assert.AreEqual("Van Huizen", result.Achternaam);
                Assert.AreEqual("1", result.Id);
            }
        }

        [TestMethod]
        public async Task GetKlantWithBestellingIdReturnsKlant()
        {
            var klant = new Klant { Voornaam = "Hans", Achternaam = "Van Huizen", Id = "1" };

            var bestelling = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
            };

            using (var context = new BeheerContext(_options))
            {
                var mapper = new KlantDatamapper(context);
                mapper.Insert(klant);

                var bestellingmapper = new BestellingDatamapper(context);
                await bestellingmapper.Insert(bestelling);

                
            }

            using (var context = new BeheerContext(_options))
            {
                var mapper = new KlantDatamapper(context);
                var resultKlant = await mapper.GetKlantWithBestellingId(1);

                Assert.AreEqual("1", resultKlant.Id);
                Assert.AreEqual("Van Huizen", resultKlant.Achternaam);
                Assert.AreEqual("Hans", resultKlant.Voornaam);
            }
        }

        [TestMethod]
        public async Task GetUnfinishedBestellingenOfKlant()
        {
            var klant = new Klant { Voornaam = "Hans", Achternaam = "Van Huizen", Id = "1" };


            var bestelling1 = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
            };

            var bestelling2 = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Middenhoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
            };

            var bestelling3 = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Hooghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.InBehandelingDoorMagazijn,
            };

            using (var context = new BeheerContext(_options))
            {
                var mapper = new KlantDatamapper(context);
                mapper.Insert(klant);

                var bestellingmapper = new BestellingDatamapper(context);
                await bestellingmapper.Insert(bestelling1);
                await bestellingmapper.Insert(bestelling2);
                await bestellingmapper.Insert(bestelling3);

            }

            using (var context = new BeheerContext(_options))
            {
                var mapper = new KlantDatamapper(context);
                var bestellingen = await mapper.GetUnFinishedBestellingenOfKlant("1");

                Assert.AreEqual(2, bestellingen.Count);
                Assert.IsTrue(bestellingen.Any(b => b.Plaats == "Laaghoven"));
                Assert.IsTrue(bestellingen.Any(b => b.Plaats == "Middenhoven"));

                Assert.IsFalse(bestellingen.Any(b => b.Plaats == "Hooghoven"));

            }
        }
    }
}