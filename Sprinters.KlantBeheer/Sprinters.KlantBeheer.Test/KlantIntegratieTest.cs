using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn;
using Minor.Nijn.TestBus;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
using Minor.Nijn.WebScale.Events;
using RabbitMQ.Client;
using Sprinters.KlantBeheer.DAL;
using Sprinters.KlantBeheer.Listeners;
using Sprinters.SharedTypes.KlantService.Commands;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.KlantBeheer.Test
{
    [TestClass]
    public class KlantIntegratieTest
    {
        private TestBusContext _context;
        private MicroserviceHost _host;
        private SqliteConnection connection;
        private DbContextOptions<KlantContext> options;


        [TestInitialize]
        public void Initialize()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            options = new DbContextOptionsBuilder<KlantContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new KlantContext(options))
            {
                context.Database.EnsureCreated();
            }


            _context = new TestBusContext();

            var services = new ServiceCollection();
            services.AddTransient<ICommandPublisher, CommandPublisher>();
            services.AddSingleton<DbContextOptions>(options);
            services.AddSingleton<KlantContext>();
            services.AddTransient<IKlantDatamapper, KlantDatamapper>();
            services.AddSingleton<IBusContext<IConnection>>(_context);
            services.AddTransient<IEventPublisher, EventPublisher>();

            var builder = new MicroserviceHostBuilder()
                .RegisterDependencies(services)
                .WithContext(_context)
                .AddCommandListener<KlantListener>();

            _host = builder.CreateHost();

            _host.StartListening();
            Thread.Sleep(1000);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _host.Dispose();
        }

        [TestMethod]
        public async Task RegistreerKlantCommandAddsKlantToDatabaseAndThrowsEvent()
        {
            _context.DeclareQueue("klantGeregistreedQueue", new List<string> {NameConstants.KlantGeregistreerdEvent});
            var command = new RegistreerKlantCommand
            {
                AccountId = "1",
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                AdresRegel = "Voorstraat 8",
                Plaats = "Groningen",
                Postcode = "1345df",
                Telefoonnummer = "0665234365"
            };

            var publisher = new CommandPublisher(_context);
            var result = await publisher.Publish<int>(command, NameConstants.RegistreerKlantCommandQueue);

            Thread.Sleep(1000);

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, _context.TestQueues["klantGeregistreedQueue"].Queue.Count);
        }
    }
}