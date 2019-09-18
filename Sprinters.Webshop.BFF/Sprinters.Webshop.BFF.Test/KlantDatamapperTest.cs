using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class KlantDatamapperTest
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
        public async Task GetKlantByIdReturnsKlant()
        {
            var klant = new Klant
            {
                Id = "1",
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                AdresRegel = "Voorstraat 8",
                Plaats = "Groningen",
                Postcode = "1345df",
                Telefoonnummer = "0665234365",
            };

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new KlantDatamapper(context);
                dataMapper.Insert(klant);
            }

            using (var context = new WebshopContext(options))
            {
                var klantmapper = new KlantDatamapper(context);
                var result = await klantmapper.GetKlant("1");
                Assert.IsNotNull(result);
                Assert.AreEqual("Hans", result.Voornaam);
                Assert.AreEqual("van Huizen", result.Achternaam);
                Assert.AreEqual("Voorstraat 8", result.AdresRegel);
                Assert.AreEqual("Groningen", result.Plaats);
                Assert.AreEqual("1345df", result.Postcode);
                Assert.AreEqual("0665234365", result.Telefoonnummer);
            }
        }

        [TestMethod]
        public async Task UpdateKlantUpdatesItIntoDatabase()
        {
            var klant = new Klant
            {
                Id = "1",
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                AdresRegel = "Voorstraat 8",
                Plaats = "Groningen",
                Postcode = "1345df",
                Telefoonnummer = "0665234365",
            };

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new KlantDatamapper(context);
                dataMapper.Insert(klant);
            }

            klant.Krediet = 340;

            using (var context = new WebshopContext(options))
            {
                var dataMapper = new KlantDatamapper(context);
                dataMapper.Update(klant);
            }

            using (var context = new WebshopContext(options))
            {
                var klantmapper = new KlantDatamapper(context);
                var result = await klantmapper.GetKlant("1");
                Assert.IsNotNull(result);
                Assert.AreEqual("Hans", result.Voornaam);
                Assert.AreEqual("van Huizen", result.Achternaam);
                Assert.AreEqual("Voorstraat 8", result.AdresRegel);
                Assert.AreEqual("Groningen", result.Plaats);
                Assert.AreEqual("1345df", result.Postcode);
                Assert.AreEqual("0665234365", result.Telefoonnummer);
                Assert.AreEqual(340, result.Krediet);
            }
        }

    }
}
