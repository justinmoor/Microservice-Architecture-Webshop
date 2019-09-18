using System;
using System.Linq;
using System.Threading;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.TestBus;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Events;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;
using Sprinters.Webshop.BFF.Listeners;

namespace Sprinters.Webshop.BFF.Test.Integratie
{
    [TestClass]
    public class MagazijnListenerIntegratieTest
    {
        private TestBusContext _context;
        private MicroserviceHost _host;
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

            var services = new ServiceCollection();
            services.AddSingleton<DbContextOptions>(options);
            services.AddTransient<WebshopContext, WebshopContext>();
            services.AddTransient<IArtikelDatamapper, ArtikelDatamapper>();
            services.AddTransient<IKlantDatamapper, KlantDatamapper>();

            _context = new TestBusContext();

            var builder = new MicroserviceHostBuilder()
                .RegisterDependencies(services)
                .WithContext(_context)
                .AddEventListener<MagazijnListener>();

            _host = builder.CreateHost();

            _host.StartListening();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _host.Dispose();
        }

        [TestMethod]
        public void InsertArtikelFromEvent()
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

            var eventPublisher = new EventPublisher(_context);
            eventPublisher.Publish(artikelEvent);

            Thread.Sleep(500);

            using (var context = new WebshopContext(options))
            {
                var result = context.Artikelen.SingleOrDefault(a => a.Artikelnummer == 1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(0, result.Voorraad);
            }
        }


        [TestMethod]
        public void ChangeVoorraadEvent()
        {
            using (var context = new WebshopContext(options))
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
                new ArtikelDatamapper(context).Insert(artikel);
            }

            var voorraadEvent = new VoorraadVerhoogdEvent
            {
                Artikelnummer = 1234,
                Aantal = 5,
                NieuweVoorraad = 5
            };


            var eventPublisher = new EventPublisher(_context);
            eventPublisher.Publish(voorraadEvent);

            Thread.Sleep(500);

            using (var context = new WebshopContext(options))
            {
                var result = context.Artikelen.SingleOrDefault(a => a.Artikelnummer == 1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(5, result.Voorraad);
            }

            var voorraadVerlaagdEvent = new VoorraadVerlaagdEvent
            {
                Artikelnummer = 1234,
                Aantal = 3,
                NieuweVoorraad = 2
            };

            eventPublisher.Publish(voorraadVerlaagdEvent);

            Thread.Sleep(500);

            using (var context = new WebshopContext(options))
            {
                var result = context.Artikelen.SingleOrDefault(a => a.Artikelnummer == 1234);
                Assert.IsNotNull(result);
                Assert.AreEqual("Fiets", result.Naam);
                Assert.AreEqual(2, result.Voorraad);
            }
        }
    }
}