using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Minor.Nijn;
using Minor.Nijn.RabbitMQBus;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
using RabbitMQ.Client;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;

namespace Sprinters.Webshop.BFF
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private static readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private RabbitMQBusContext _context;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ConfigureAuthentication(services);
            CreateNijnHostBuilder(services);
        }

        public void ConfigureAuthentication(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("ISSUER"),
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKEY"))),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            app.UseAuthentication();
            app.UseMvc();
        }

        public void CreateNijnHostBuilder(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SPRINTERDB");
            var options = new DbContextOptionsBuilder<WebshopContext>()
                .UseNpgsql(connectionString)
                .Options;

            services.AddTransient<ICommandPublisher, CommandPublisher>();
            services.AddSingleton<DbContextOptions>(options);
            services.AddTransient<WebshopContext, WebshopContext>();
            services.AddTransient<IArtikelDatamapper, ArtikelDatamapper>();
            services.AddTransient<IBestellingDatamapper, BestellingDatamapper>();
            services.AddTransient<IKlantDatamapper, KlantDatamapper>();

            var nijnContext = CreateNijnConfig(services);

            using (var context = new WebshopContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                if (!context.Artikelen.Any()) ScrapeAuditLog(services, nijnContext, DateTime.Now).Wait();
            }

            services.AddSingleton(nijnContext);
        }

        private void OnShutdown()
        {
            _context.Dispose();
        }

        public async Task ScrapeAuditLog(IServiceCollection serviceCollection, IBusContext<IConnection> nijnContext,
            DateTime startTime)
        {
            var exchangeName = "BFF_Webshop " + Guid.NewGuid();

            var connectionBuilder = new RabbitMQContextBuilder()
                .ReadFromEnvironmentVariables().WithExchange(exchangeName);

            var context = connectionBuilder.CreateContext();

            var builder = new MicroserviceHostBuilder()
                .RegisterDependencies(serviceCollection)
                .WithContext(context)
                .UseConventions()
                .ExitAfterIdleTime(new TimeSpan(0, 0, 5, 0));

            builder.CreateHost().StartListeningInOtherThread();

            var publisher = new CommandPublisher(nijnContext);

            var replayEventsCommand = new ReplayEventsCommand
            {
                ToTimestamp = startTime.Ticks,
                ExchangeName = exchangeName
            };

            var result = await publisher.Publish<bool>(replayEventsCommand, "AuditlogReplayService",
                "Minor.WSA.AuditLog.Commands.ReplayEventsCommand");

            Console.WriteLine(result);
        }

        private IBusContext<IConnection> CreateNijnConfig(IServiceCollection serviceCollection)
        {
            //Deprecated method, maar kan even niet anders
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(
                new ConsoleLoggerProvider(
                    (text, logLevel) => logLevel >= LogLevel.Debug, true));

            var connectionBuilder = new RabbitMQContextBuilder()
                .ReadFromEnvironmentVariables();

            _context = connectionBuilder.CreateContext();

            var builder = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .RegisterDependencies(serviceCollection)
                .WithContext(_context)
                .UseConventions();

            var host = builder.CreateHost();
            host.StartListeningInOtherThread();

            return _context;
        }
    }
}