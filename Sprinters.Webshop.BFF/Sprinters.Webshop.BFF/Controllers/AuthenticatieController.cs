using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
using Sprinters.SharedTypes.Authenticatie.Commands;
using Sprinters.SharedTypes.Authenticatie.Entities;
using Sprinters.SharedTypes.Authenticatie.Exceptions;
using Sprinters.SharedTypes.Authenticatie.Roles;
using Sprinters.SharedTypes.KlantService.Commands;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatieController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly ILogger _logger;

        public AuthenticatieController(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
            _logger = NijnLogger.CreateLogger<AuthenticatieController>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            JwtResult jwtResult;

            try
            {
                jwtResult = await _commandPublisher.Publish<JwtResult>(
                    new LogGebruikerInCommand {Credentials = credentials},
                    NameConstants.LogGebuikerInCommandQueue);
            }
            catch (LoginFailedException)
            {
                return Unauthorized();
            }

            return Ok(jwtResult);
        }

        [HttpPost("registreer")]
        public async Task<IActionResult> RegistreerKlant(Klant klant)
        {
            var registreerKlantCommand = new RegistreerKlantCommand
            {
                Voornaam = klant.Voornaam,
                Achternaam = klant.Achternaam,
                AdresRegel = klant.AdresRegel,
                Plaats = klant.Plaats,
                Postcode = klant.Postcode,
                Telefoonnummer = klant.Telefoonnummer
            };

            var registreerGebruikerCommand = new RegistreerGebruikerCommand
            {
                NewUser = new Account
                {
                    UserName = klant.Email,
                    Password = klant.Wachtwoord,
                    Role = Roles.Klant
                }
            };

            try
            {
                var registreerGebruikerResult = await _commandPublisher.Publish<string>(registreerGebruikerCommand,
                    NameConstants.RegistreerGebruikerCommandQueue);
                registreerKlantCommand.AccountId = registreerGebruikerResult;
            }
            catch (NoResponseException)
            {
                _logger.LogDebug("Authenticatie service unavaible");
                return StatusCode(503, "Service niet bereikbaar.");
            }
            catch (AccountCreationException)
            {
                return StatusCode(409, "Account aanmaken is verkeerd gegaan.");
            }
            catch (PasswordException)
            {
                return StatusCode(412, "Wachtwoord niet sterk genoeg.");
            }
            catch (UsernameAlreadyExistsException)
            {
                return StatusCode(409, "Email bestaat al.");
            }

            try
            {
                var registreerKlantResult = await _commandPublisher.Publish<string>(registreerKlantCommand,
                    NameConstants.RegistreerKlantCommandQueue);
            }
            catch (NoResponseException)
            {
                await RollbackGeregristreerdAccount(registreerKlantCommand.AccountId);
                _logger.LogDebug("Klantbeheer service unavaible");
                return StatusCode(503);
            }
            catch (Exception)
            {
                await RollbackGeregristreerdAccount(registreerKlantCommand.AccountId);
                _logger.LogDebug("Account aanmaken mislukt rollback wordt gedaan. Met Accountid : {}",
                    registreerKlantCommand.AccountId);
                return StatusCode(500, "Account aanmaken mislukt.");
            }

            return Ok();
        }


        private async Task RollbackGeregristreerdAccount(string id)
        {
            try
            {
                var verwijderAccount = await _commandPublisher.Publish<string>(new VerwijderGebruikerCommand
                {
                    Id = id
                }, NameConstants.VerwijderGebruikerCommandQueue);
            }
            catch (UserDeletionFailedException)
            {
                _logger.LogError("Account is verwijderd.");
            }
        }
    }
}