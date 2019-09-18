using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprinters.Authenticatie.DAL;
using Sprinters.Authenticatie.Listeners;
using Sprinters.SharedTypes.Authenticatie.Commands;
using Sprinters.SharedTypes.Authenticatie.Entities;
using Sprinters.SharedTypes.Authenticatie.Exceptions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace Sprinters.Authenticatie.Test
{
    [TestClass]
    public class AccountCommandListenerTest
    {
        private IServiceCollection _services;
        private SqliteConnection _connection;

        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        [TestInitialize]
        public void Initialize()
        {
            _services = new ServiceCollection();

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _services.AddDbContext<AccountsContext>(builder =>
                builder.UseSqlite(_connection));

            _services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AccountsContext>()
                .AddDefaultTokenProviders();

            var serviceProvider = _services.BuildServiceProvider();

            var context = serviceProvider.GetService<AccountsContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            _userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            _signInManager = serviceProvider.GetService<SignInManager<IdentityUser>>();
            _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            _roleManager.CreateAsync(new IdentityRole() { Name = "Klant" }).Wait();
            _roleManager.CreateAsync(new IdentityRole() { Name = "Magazijn" }).Wait();
            _roleManager.CreateAsync(new IdentityRole() { Name = "Sales" }).Wait();

            Environment.SetEnvironmentVariable("JWTKEY", "SuperBelangrijkeSleutel101");
            Environment.SetEnvironmentVariable("ISSUER", "kantilever.nl");
        }

        [TestCleanup]
        public void CleanUp()
        {
            Environment.SetEnvironmentVariable("JWTKEY", null);
            Environment.SetEnvironmentVariable("ISSUER", null);
        }

        [TestMethod]
        public async Task NewUser_CanBe_Registered()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var command = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "klaas@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            var result = await listener.RegistreerGebruiker(command);

            Assert.IsNotNull(result);            
        }

        [TestMethod]
        public async Task ThrowsPasswordException_WhenPassword_IsInvalid()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var command = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "slecht",
                    Role = "Klant"
                }
            };

            await Assert.ThrowsExceptionAsync<PasswordException>(async () => {
                await listener.RegistreerGebruiker(command);
            });
        }

        [TestMethod]
        public async Task ThrowsUdernameAlreadyExistsException_WhenUsernameAlreadyExists()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var command = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // eerste users
            await listener.RegistreerGebruiker(command);
   
            // tweede dubbele user
            await Assert.ThrowsExceptionAsync<UsernameAlreadyExistsException>(async () => {
                await listener.RegistreerGebruiker(command);
            });
        }

        [TestMethod]
        public async Task GeneratesJwt_WhenSuccesfullLogin()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var registerCommand = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            await listener.RegistreerGebruiker(registerCommand);

            var loginCommand = new LogGebruikerInCommand() { Credentials = new Credentials() { UserName = "hallo@live.nl", Password = "GeheimWW83!" } };

            var result = await listener.LogGebruikerIn(loginCommand);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ThrowException_WhenWrongUsername()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var registerCommand = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            await listener.RegistreerGebruiker(registerCommand);

            var loginCommand = new LogGebruikerInCommand() { Credentials = new Credentials() { UserName = "verkeerd@live.nl", Password = "verkeerd!" } };

            await Assert.ThrowsExceptionAsync<LoginFailedException>(async () => {
                await listener.LogGebruikerIn(loginCommand);
            });
        }

        [TestMethod]
        public async Task ThrowException_WhenWrongPassword()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var registerCommand = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            await listener.RegistreerGebruiker(registerCommand);

            var loginCommand = new LogGebruikerInCommand() { Credentials = new Credentials() { UserName = "hallo@live.nl", Password = "verkeerd!" } };

            await Assert.ThrowsExceptionAsync<LoginFailedException>(async () => {
                await listener.LogGebruikerIn(loginCommand);
            });
        }

        [TestMethod]
        public async Task Jwt_Contains_CorrectRoles()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var registerCommand = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            await listener.RegistreerGebruiker(registerCommand);

            var loginCommand = new LogGebruikerInCommand() { Credentials = new Credentials() { UserName = "hallo@live.nl", Password = "GeheimWW83!" } };

            var result = await listener.LogGebruikerIn(loginCommand);

            var jwtHandler = new JwtSecurityTokenHandler();
            bool validToken = jwtHandler.CanReadToken(result.Token);

            Assert.IsTrue(validToken);

            var token = jwtHandler.ReadJwtToken(result.Token);
            var role = token.Claims.Where(c => c.Type == ClaimTypes.Role && c.Value == "Klant");

            Assert.IsNotNull(role);
        }

        [TestMethod]
        public async Task Jwt_Contains_CorrectId()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var registerCommand = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            string id = await listener.RegistreerGebruiker(registerCommand);

            var loginCommand = new LogGebruikerInCommand() { Credentials = new Credentials() { UserName = "hallo@live.nl", Password = "GeheimWW83!" } };

            var result = await listener.LogGebruikerIn(loginCommand);

            var jwtHandler = new JwtSecurityTokenHandler();
            bool validToken = jwtHandler.CanReadToken(result.Token);

            Assert.IsTrue(validToken);

            var token = jwtHandler.ReadJwtToken(result.Token);
            var idFromClaim = token.Claims.Where(c => c.Type == "UserId" && c.Value == id).SingleOrDefault();

            Assert.AreEqual(id, idFromClaim.Value);
        }

        [TestMethod]
        public async Task CanSuccesfully_Delete_AnUser()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var command = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            string id = await listener.RegistreerGebruiker(command);

            var result = await listener.VerwijderGebruiker(new VerwijderGebruikerCommand() { Id = id });

            Assert.AreEqual(id, result);
        }

        [TestMethod]
        public async Task ThrowsException_When_CantDeleteUser()
        {
            AccountCommandListener listener = new AccountCommandListener(_signInManager, _userManager);

            var command = new RegistreerGebruikerCommand()
            {
                NewUser = new Account()
                {
                    UserName = "hallo@live.nl",
                    Password = "GeheimWW83!",
                    Role = "Klant"
                }
            };

            // maak user aan
            string id = await listener.RegistreerGebruiker(command);

            await Assert.ThrowsExceptionAsync<UserDeletionFailedException>(async () => {
                await listener.VerwijderGebruiker(new VerwijderGebruikerCommand() { Id = "wrongId" });
            });
        }
    }
}
