using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn;
using Minor.Nijn.TestBus;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Attributes;
using Minor.Nijn.WebScale.Commands;
using Minor.Nijn.WebScale.Events;
using RabbitMQ.Client;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.BestellingBeheer.Listeners;
using Sprinters.SharedTypes.BeheerService.Commands;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.BestellingBeheer.Test.Integratie
{
    [CommandListener]
    [TestClass]
    public class BestellingListenerIntegratieTest
    {
        private static int _magazijnCalled;
        private TestBusContext _context;
        private MicroserviceHost _host;
        private SqliteConnection connection;
        private DbContextOptions<BeheerContext> options;

        [TestInitialize]
        public void Initialize()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            options = new DbContextOptionsBuilder<BeheerContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new BeheerContext(options))
            {
                context.Database.EnsureCreated();
                var artikel = new Artikel
                {
                    Artikelnummer = 1,
                    Beschrijving = "Grote fiets voor iedereen",
                    LeverbaarTot = new DateTime(2018, 5, 5),
                    LeverbaarVanaf = new DateTime(2017, 1, 1),
                    Naam = "Fiets",
                    Prijs = 40m,
                    Voorraad = 5
                };
                var artikel2 = new Artikel
                {
                    Artikelnummer = 2,
                    Beschrijving = "HELE grote fiets voor iedereen",
                    LeverbaarTot = new DateTime(2018, 5, 5),
                    LeverbaarVanaf = new DateTime(2017, 1, 1),
                    Naam = "Fiets Groot",
                    Prijs = 100m,
                    Voorraad = 8
                };
                var klant = new Klant {Voornaam = "Hans", Achternaam = "Van Huizen", Id = "1"};

                var klantDatamapper = new KlantDatamapper(context);
                klantDatamapper.Insert(klant);

                var datamapper = new ArtikelDatamapper(context);
                datamapper.Insert(artikel);
                datamapper.Insert(artikel2);
            }


            _context = new TestBusContext();

            var services = new ServiceCollection();
            services.AddTransient<ICommandPublisher, CommandPublisher>();
            services.AddSingleton<DbContextOptions>(options);
            services.AddTransient<BeheerContext, BeheerContext>();
            services.AddTransient<IArtikelDatamapper, ArtikelDatamapper>();
            services.AddTransient<IBestellingDatamapper, BestellingDatamapper>();
            services.AddSingleton<IBusContext<IConnection>>(_context);
            services.AddTransient<IEventPublisher, EventPublisher>();
            services.AddTransient<IKlantDatamapper, KlantDatamapper>();

            var builder = new MicroserviceHostBuilder()
                .RegisterDependencies(services)
                .WithContext(_context)
                .AddCommandListener<BestellingListener>()
                .AddCommandListener<BestellingListenerIntegratieTest>()
                .AddEventListener<BestellingListener>();

            _host = builder.CreateHost();

            _host.StartListening();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _host.Dispose();
        }

        [TestMethod]
        public async Task InsertNieuweBestellingUnder500EuroFromEventSetsStatusToGereed()
        {
            var bestelling = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1,1),
                    new BestellingItem(2,1)
                }
            };


            var commandPublisher = new CommandPublisher(_context);
            await commandPublisher.Publish<bool>(bestelling, NameConstants.NieuweBestellingCommandQueue);

            Thread.Sleep(500);

            using (var context = new BeheerContext(options))
            {
                var datamapper = new BestellingDatamapper(context);
                var result = await datamapper.GetBestelling(1);
                Assert.AreEqual("Hans", result.Klant.Voornaam);
                Assert.AreEqual("Van Huizen", result.Klant.Achternaam);
                Assert.AreEqual("1", result.KlantId);
                Assert.AreEqual("Laagstraat 11", result.AdresRegel1);
                Assert.AreEqual("Laaghoven", result.Plaats);
                Assert.AreEqual("1234FG", result.Postcode);
                Assert.AreEqual(2, result.BesteldeArtikelen.Count);
                Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, result.BestellingStatus);
                Assert.IsTrue(result.BesteldeArtikelen.Any(b => b.Artikel.Naam == "Fiets" && b.Aantal == 1));

                var klantMapper = new KlantDatamapper(context);
                var klant = await klantMapper.GetKlant("1");
                Assert.AreEqual(169.4m, klant.KredietMetSales);
            }
        }

        [TestMethod]
        public async Task NieuweBestellingThatGoesAboveKredietLimitGoesToSale()
        {
            var bestelling = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 3)
                }
            };


            var commandPublisher = new CommandPublisher(_context);
            await commandPublisher.Publish<bool>(bestelling, NameConstants.NieuweBestellingCommandQueue);

            Thread.Sleep(500);

            using (var context = new BeheerContext(options))
            {
                var datamapper = new BestellingDatamapper(context);
                var result = await datamapper.GetBestelling(1);
                Assert.AreEqual("Hans", result.Klant.Voornaam);
                Assert.AreEqual("Van Huizen", result.Klant.Achternaam);
                Assert.AreEqual("1", result.KlantId);
                Assert.AreEqual("Laagstraat 11", result.AdresRegel1);
                Assert.AreEqual("Laaghoven", result.Plaats);
                Assert.AreEqual("1234FG", result.Postcode);
                Assert.AreEqual(2, result.BesteldeArtikelen.Count);
                Assert.AreEqual(BestellingStatus.TerControleVoorSales, result.BestellingStatus);
                Assert.IsTrue(result.BesteldeArtikelen.Any(b => b.Artikel.Naam == "Fiets" && b.Aantal == 3));

                var klantMapper = new KlantDatamapper(context);
                var klant = await klantMapper.GetKlant("1");

                //Krediet should not be added yet
                Assert.AreEqual(508.2m, klant.KredietMetSales);
            }

        }

        [TestMethod]
        public async Task VolgendeBestellingInpakkenGivesVolgendeBestellingSetsStatusToInbehandeling()
        {
            using (var context = new BeheerContext(options))
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
                var bestelling2 = new Bestelling
                {
                    BestellingStatus = BestellingStatus.GereedVoorBehandeling,
                    KlantId = "1",
                    AdresRegel1 = "Hoogstraat 77",
                    Plaats = "Hooghoven",
                    Postcode = "4321FE",
                    BesteldeArtikelen = new List<BestellingItem>
                    {
                        new BestellingItem(1, 3),
                        new BestellingItem(2, 5)
                    }
                };

                var datamapper = new BestellingDatamapper(context);
                await datamapper.Insert(bestelling);
                await datamapper.Insert(bestelling2);
            }

            var commandPublisher = new CommandPublisher(_context);
            var result = await commandPublisher.Publish<int>(new VolgendeBestellingCommand(),
                NameConstants.VolgendeBestellingCommandQueue);

            Assert.AreEqual(1, result);

            var result2 = await commandPublisher.Publish<int>(new VolgendeBestellingCommand(),
                NameConstants.VolgendeBestellingCommandQueue);

            Assert.AreEqual(2, result2);

            var resultShouldbeZero = await commandPublisher.Publish<int>(new VolgendeBestellingCommand(),
                NameConstants.VolgendeBestellingCommandQueue);

            Assert.AreEqual(0, resultShouldbeZero);


            using (var context = new BeheerContext(options))
            {
                var bestellingDatamapper = new BestellingDatamapper(context);
                var bestelling = await bestellingDatamapper.GetBestelling(1);
                Assert.AreEqual(BestellingStatus.InBehandelingDoorMagazijn, bestelling.BestellingStatus);

            }
        }


        [TestMethod]
        public async Task FinishBestellingLowersVoorraad()
        {
            _magazijnCalled = 0;

            using (var context = new BeheerContext(options))
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
                var datamapper = new BestellingDatamapper(context);
                await datamapper.Insert(bestelling);
            }


            var commandPublisher = new CommandPublisher(_context);
            var result = await commandPublisher.Publish<int>(new BestellingAfrondenCommand {Id = 1},
                NameConstants.FinishBestellingCommandQueue);
            Thread.Sleep(1000);

            Assert.AreEqual(2, _magazijnCalled);
        }

        [TestMethod]
        public async Task KeurBestellingGoedChangesBestellingToGereedVoorBehandeling()
        {
            using (var context = new BeheerContext(options))
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
                var datamapper = new BestellingDatamapper(context);
                await datamapper.Insert(bestelling);
            }

            var commandPublisher = new CommandPublisher(_context);
            await commandPublisher.Publish<int>(new BestellingGoedkeurenCommand {Id = 1},
                NameConstants.BestellingGoedKeurenCommandQueue);
            Thread.Sleep(1000);

            using (var context = new BeheerContext(options))
            {
                var datamapper = new BestellingDatamapper(context);
                var result = await datamapper.GetBestelling(1);
                Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, result.BestellingStatus);

                var klantmapper = new KlantDatamapper(context);
                var klant = await  klantmapper.GetKlant("1");

            }
        }

        [Command("Kantilever.MagazijnService")]
        public bool MagazijnVoorraadMock(HaalVoorraadUitMagazijnCommand command)
        {
            _magazijnCalled++;
            return true;
        }
    }
}