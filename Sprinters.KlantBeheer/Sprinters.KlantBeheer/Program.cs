using System;
using System.Diagnostics.CodeAnalysis;
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
using Sprinters.KlantBeheer.DAL;

namespace Sprinters.KlantBeheer
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        private static void Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("SPRINTERDB");
            var options = new DbContextOptionsBuilder<KlantContext>()
                .UseNpgsql(connectionString)
                .Options;

            //Deprecated method, maar kan even niet anders
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
            services.AddTransient<IKlantDatamapper, KlantDatamapper>();
            services.AddSingleton<DbContextOptions>(options);
            services.AddTransient<KlantContext, KlantContext>();
            services.AddSingleton<IBusContext<IConnection>>(nijnContext);

            var builder = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .RegisterDependencies(services)
                .WithContext(nijnContext)
                .UseConventions();

            var host = builder.CreateHost();
            host.CreateQueues();

            using (var context = new KlantContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            host.StartListeningInOtherThread();

            var logger = NijnLogger.CreateLogger<Program>();
            logger.LogInformation("Started...");
        }
    }
}