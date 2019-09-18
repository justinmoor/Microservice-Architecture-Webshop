using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprinters.KlantBeheer.DAL;
using Sprinters.SharedTypes.KlantService.Entities;

namespace Sprinters.KlantBeheer.Test
{
    [TestClass]
    public class KlantDatamapperTest
    {
        private SqliteConnection _connection;
        private DbContextOptions<KlantContext> _options;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<KlantContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new KlantContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestMethod]
        public async Task InsertAddsKlantToDatabase()
        {
            var klant = new Klant
            {
                Id = "1",
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                AdresRegel = "Voorstraat 8",
                Plaats = "Groningen",
                Postcode = "1345df",
                Telefoonnummer = "0665234365"
            };

            using (var context = new KlantContext(_options))
            {
                var dataMapper = new KlantDatamapper(context);
                await dataMapper.Insert(klant);
            }

            using (var context = new KlantContext(_options))
            {
                var result = context.Klanten.SingleOrDefault(a => a.Id == "1");
                Assert.IsNotNull(result);
                Assert.AreEqual("Hans", result.Voornaam);
                Assert.AreEqual("van Huizen", result.Achternaam);
                Assert.AreEqual("Voorstraat 8", result.AdresRegel);
                Assert.AreEqual("Groningen", result.Plaats);
                Assert.AreEqual("1345df", result.Postcode);
                Assert.AreEqual("0665234365", result.Telefoonnummer);
            }
        }
    }
}