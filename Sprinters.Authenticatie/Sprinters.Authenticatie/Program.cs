using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Minor.Nijn;
using Minor.Nijn.RabbitMQBus;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
using Minor.Nijn.WebScale.Events;
using Sprinters.Authenticatie.DAL;
using System;
using Microsoft.AspNetCore.Identity;
using Sprinters.SharedTypes.Authenticatie.Roles;
using System.Diagnostics.CodeAnalysis;

namespace Sprinters.Authenticatie
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            ConfigureDatabase(services);
            ConfigureAuthentication(services);
            ConfigureNijn(services);
        }

        private static void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AccountsContext>()
                .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            roleManager.CreateAsync(new IdentityRole() { Name = Roles.Klant }).Wait();
            roleManager.CreateAsync(new IdentityRole() { Name = Roles.Magazijn }).Wait(); 
            roleManager.CreateAsync(new IdentityRole() { Name = Roles.Sales }).Wait();

            RegisterStandardUsers(serviceProvider);
        }

        private static void RegisterStandardUsers(ServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var magazijnEmail = Environment.GetEnvironmentVariable("MAGAZIJNEMAIL");
            var magazijnPassword = Environment.GetEnvironmentVariable("MAGAZIJNPASSWORD");

            if (!string.IsNullOrEmpty(magazijnEmail))
            {
                var magazijnMedewerker = new IdentityUser() { UserName = magazijnEmail, Email = magazijnEmail };

                userManager.CreateAsync(magazijnMedewerker, magazijnPassword).Wait();
                userManager.AddToRoleAsync(magazijnMedewerker, Roles.Magazijn);
            }

            var salesEmail = Environment.GetEnvironmentVariable("SALESEMAIL");
            var salesPassword = Environment.GetEnvironmentVariable("SALESPASSWORD");

            if (!string.IsNullOrEmpty(salesEmail))
            { 
                var salesMedewerker = new IdentityUser() { UserName = salesEmail, Email = salesEmail };

                userManager.CreateAsync(salesMedewerker, salesPassword).Wait();
                userManager.AddToRoleAsync(salesMedewerker, Roles.Sales);
            }

        }

        private static void ConfigureNijn(IServiceCollection services)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(
                new ConsoleLoggerProvider(
                    (text, logLevel) => logLevel >= LogLevel.Debug, true));

            var connectionBuilder = new RabbitMQContextBuilder()
                .ReadFromEnvironmentVariables();

            var nijnContext = connectionBuilder.CreateContext();

            services.AddTransient<ICommandPublisher, CommandPublisher>();
            services.AddTransient<IEventPublisher, EventPublisher>();

            var builder = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .RegisterDependencies(services)
                .WithContext(nijnContext)
                .UseConventions();

            var host = builder.CreateHost();
            host.CreateQueues();

            host.StartListeningInOtherThread();

            var logger = NijnLogger.CreateLogger<Program>();
            logger.LogInformation("Started...");
        }

        private static void ConfigureDatabase(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SPRINTERDB");

            services.AddDbContext<AccountsContext>(builder =>
                builder.UseNpgsql(connectionString));

            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetService<AccountsContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
    
}
