using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale.Attributes;
using Sprinters.Authenticatie.Security;
using Sprinters.SharedTypes.Authenticatie.Commands;
using Sprinters.SharedTypes.Authenticatie.Entities;
using Sprinters.SharedTypes.Authenticatie.Exceptions;
using Sprinters.SharedTypes.Authenticatie.Roles;
using Sprinters.SharedTypes.Shared;
using System.Threading.Tasks;

namespace Sprinters.Authenticatie.Listeners
{
    [CommandListener]
    public class AccountCommandListener
    {
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        public AccountCommandListener(SignInManager<IdentityUser> signinManager, UserManager<IdentityUser> userManager)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _logger = NijnLogger.CreateLogger<AccountCommandListener>();
        }

        [Command(NameConstants.VerwijderGebruikerCommandQueue)]
        public async Task<string> VerwijderGebruiker(VerwijderGebruikerCommand verwijderGebruikerCommand)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(usr => usr.Id == verwijderGebruikerCommand.Id);

            if(user == null)
            {
                _logger.LogWarning("Can't delete user with id {id}.", verwijderGebruikerCommand.Id);
                throw new UserDeletionFailedException("Can't delete user.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Can't delete user with id {id} and username {username}.", user.Id, user.UserName);
                throw new UserDeletionFailedException("Can't delete user.");
            }

            return user.Id;
        }

        [Command(NameConstants.LogGebuikerInCommandQueue)]
        public async Task<JwtResult> LogGebruikerIn(LogGebruikerInCommand logGebruikerInCommand)
        {
            var creds = logGebruikerInCommand.Credentials;

            var user = await _userManager.Users.SingleOrDefaultAsync(usr => usr.UserName == creds.UserName);

            if(user == null)
            {
                _logger.LogInformation("Failed login for {user}", creds.UserName);
                throw new LoginFailedException("Wrong username or password.");
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, creds.Password, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                return new JwtResult()
                {
                    Token = JwtUtil.Generate(user, roles)
                };
            } 
            else
            {
                _logger.LogInformation("Failed login for {user}", creds.UserName);
                throw new LoginFailedException("Wrong username or password.");
            }
        }

        [Command(NameConstants.RegistreerGebruikerCommandQueue)]
        public async Task<string> RegistreerGebruiker(RegistreerGebruikerCommand registreerGebruikerCommand)
        {
            // Email = username
            var username = registreerGebruikerCommand.NewUser.UserName;

            var newUser = new IdentityUser() { UserName = username, Email = username };

            var newUserResult = await _userManager.CreateAsync(newUser, registreerGebruikerCommand.NewUser.Password);

            if (newUserResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Roles.Klant);
                string id = await _userManager.GetUserIdAsync(newUser);

                _logger.LogInformation("New user created. Username: {username}, ID: {id} ", username, id);
                return id;
            }
            else
            {
                foreach (var error in newUserResult.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordTooShort":
                        case "PasswordRequiresNonAlphanumeric":
                        case "PasswordRequiresLower":
                        case "PasswordRequiresUpper":
                        case "PasswordRequiresDigit":
                            throw new PasswordException("Password not valid.");
                        case "DuplicateUserName":
                            throw new UsernameAlreadyExistsException("Duplicate username.");
                        default:
                            _logger.LogWarning("Account creation failed for user {username}", username);
                            throw new AccountCreationException("Something went wrong.");
                    }
                }

                _logger.LogWarning("Account creation failed for user {username}", username);
                throw new AccountCreationException("Something went wrong.");
            }
        }
    }
}

