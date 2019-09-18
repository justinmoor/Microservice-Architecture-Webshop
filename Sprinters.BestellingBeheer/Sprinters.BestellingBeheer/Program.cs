using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Minor.Nijn;
using Minor.Nijn.RabbitMQBus;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
using Minor.Nijn.WebScale.Events;
using RabbitMQ.Client;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.BestellingBeheer
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("SPRINTERDB");
            var options = new DbContextOptionsBuilder<BeheerContext>()
                .UseNpgsql(connectionString)
                .Options;

            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(
                new ConsoleLoggerProvider(
                    (text, logLevel) => logLevel >= LogLevel.Debug, true));

            var connectionBuilder = new RabbitMQContextBuilder()
                .ReadFromEnvironmentVariables();

            var nijnContext = connectionBuilder.CreateContext();

            var services = new ServiceCollection();
            services.AddTransient<ICommandPublisher, CommandPublisher>();
            services.AddTransient<IEventPublisher, EventPublisher>();
            services.AddSingleton<DbContextOptions>(options);
            services.AddTransient<BeheerContext, BeheerContext>();
            services.AddTransient<IArtikelDatamapper, ArtikelDatamapper>();
            services.AddTransient<IBestellingDatamapper, BestellingDatamapper>();
            services.AddSingleton<IBusContext<IConnection>>(nijnContext);
            services.AddTransient<IKlantDatamapper, KlantDatamapper>();

            var builder = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .RegisterDependencies(services)
                .WithContext(nijnContext)
                .UseConventions();

            var host = builder.CreateHost();
            host.CreateQueues();

            using (var context = new BeheerContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                if (!context.Artikelen.Any()) ScrapeAuditLog(nijnContext, services, DateTime.Now).Wait();
            }

            host.StartListeningInOtherThread();

            var logger = NijnLogger.CreateLogger<Program>();
            logger.LogInformation("Started...");
        }

        public static async Task ScrapeAuditLog(IBusContext<IConnection> mainContext, ServiceCollection collection,
            DateTime startTime)
        {
            var exchangeName = "Audit_Bestelling " + Guid.NewGuid();
            var connectionBuilder = new RabbitMQContextBuilder()
                .ReadFromEnvironmentVariables().WithExchange(exchangeName);

            var builder = new MicroserviceHostBuilder();

            builder
                .RegisterDependencies(collection)
                .WithContext(connectionBuilder.CreateContext())
                .ExitAfterIdleTime(new TimeSpan(0, 0, 2, 0))
                .UseConventions();

            builder.CreateHost().StartListeningInOtherThread();

            var publisher = new CommandPublisher(mainContext);

            var replayEventsCommand = new ReplayEventsCommand
            {
                ToTimestamp = startTime.Ticks,
                ExchangeName = exchangeName
            };

            var result = await publisher.Publish<bool>(replayEventsCommand, "AuditlogReplayService",
                "Minor.WSA.AuditLog.Commands.ReplayEventsCommand");
        }
    }
}