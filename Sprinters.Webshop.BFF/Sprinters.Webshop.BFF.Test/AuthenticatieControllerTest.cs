using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
using Moq;
using Sprinters.SharedTypes.Authenticatie.Commands;
using Sprinters.SharedTypes.Authenticatie.Entities;
using Sprinters.SharedTypes.Authenticatie.Exceptions;
using Sprinters.SharedTypes.KlantService.Commands;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.Controllers;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class AuthenticatieControllerTest
    {
        [TestMethod]
        public async Task NieuweKlantReturnsOk()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).ReturnsAsync("test").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).ReturnsAsync("1").Verifiable();

            var sut = new AuthenticatieController(publisherMock.Object);


            var result = await sut.RegistreerKlant(klant);

            var oKResult = result as OkResult;

            Assert.IsNotNull(oKResult);

        }

        [TestMethod]
        public async Task AuthenticatieThrowsNoResponseException()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).ReturnsAsync("test").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).Throws(new NoResponseException());

            var sut = new AuthenticatieController(publisherMock.Object);

            var result = await sut.RegistreerKlant(klant);

            var statusResult = result as ObjectResult;

            Assert.IsNotNull(statusResult);

            Assert.AreEqual(503,statusResult.StatusCode);
            Assert.AreEqual("Service niet bereikbaar.",statusResult.Value);
        }

        [TestMethod]
        public async Task AuthenticatieThrowsAccountCreationException()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).ReturnsAsync("test").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).Throws(new AccountCreationException("exception"));

            var sut = new AuthenticatieController(publisherMock.Object);

            var result = await sut.RegistreerKlant(klant);

            var statusResult = result as ObjectResult;

            Assert.IsNotNull(statusResult);

            Assert.AreEqual(409, statusResult.StatusCode);
            Assert.AreEqual("Account aanmaken is verkeerd gegaan.", statusResult.Value);
        }

        [TestMethod]
        public async Task AuthenticatieThrowsPasswordException()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).ReturnsAsync("test").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).Throws(new PasswordException("exception"));

            var sut = new AuthenticatieController(publisherMock.Object);

            var result = await sut.RegistreerKlant(klant);

            var statusResult = result as ObjectResult;

            Assert.IsNotNull(statusResult);

            Assert.AreEqual(412, statusResult.StatusCode);
            Assert.AreEqual("Wachtwoord niet sterk genoeg.", statusResult.Value);
        }

        [TestMethod]
        public async Task AuthenticatieThrowsUsernameAlreadyExistsException()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).ReturnsAsync("test").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).Throws(new UsernameAlreadyExistsException("exception"));

            var sut = new AuthenticatieController(publisherMock.Object);

            var result = await sut.RegistreerKlant(klant);

            var statusResult = result as ObjectResult;

            Assert.IsNotNull(statusResult);

            Assert.AreEqual(409, statusResult.StatusCode);
            Assert.AreEqual("Email bestaat al.", statusResult.Value);
        }

        [TestMethod]
        public async Task AuthenticatieThrowsUsernameNoResponseExceptionForAuthenticatieService()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).Throws(new NoResponseException("exception"));
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).ReturnsAsync("1").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<VerwijderGebruikerCommand>(),
                    NameConstants.VerwijderGebruikerCommandQueue, "")).ReturnsAsync("1").Verifiable();

            var sut = new AuthenticatieController(publisherMock.Object);

            var result = await sut.RegistreerKlant(klant);

            var statusResult = result as StatusCodeResult;

            Assert.IsNotNull(statusResult);

            Assert.AreEqual(503, statusResult.StatusCode);
        }

        [TestMethod]
        public async Task AuthenticatieThrowsUsernameRollbackIsCalled()
        {
            var klant = new Klant
            {
                Voornaam = "Bob",
                Achternaam = "Kuipers",
                Telefoonnummer = "0612345678",
                AdresRegel = "Van der brugghenstraat",
                Plaats = "Nijmegen",
                Postcode = "6511SL",
                Email = "bobkuipers1991@gmail.com",
                Wachtwoord = "Geheim_101"
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerKlantCommand>(),
                    NameConstants.RegistreerKlantCommandQueue, "")).Throws(new UsernameAlreadyExistsException("exception"));
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<RegistreerGebruikerCommand>(),
                    NameConstants.RegistreerGebruikerCommandQueue, "")).ReturnsAsync("1").Verifiable();
            publisherMock
                .Setup(m => m.Publish<string>(It.IsAny<VerwijderGebruikerCommand>(),
                    NameConstants.VerwijderGebruikerCommandQueue, "")).ReturnsAsync("1").Verifiable();

            var sut = new AuthenticatieController(publisherMock.Object);


            var result = await sut.RegistreerKlant(klant);
        }

        [TestMethod]
        public async Task LoginReturnsOk_WithJwt_WhenCredsValid()
        {
            JwtResult jwtMock = new JwtResult() { Token = "MockToken" };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);

            publisherMock
                .Setup(m => m.Publish<JwtResult>(It.IsAny<LogGebruikerInCommand>(),
                    NameConstants.LogGebuikerInCommandQueue, "")).ReturnsAsync(jwtMock);

            var target = new AuthenticatieController(publisherMock.Object);

            var result = await target.Login(new Credentials() { UserName = "email@.live.nl", Password = "password" });

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            JwtResult jwtResult  = (result as OkObjectResult).Value as JwtResult;

            Assert.AreEqual("MockToken", jwtResult.Token);
        }

        [TestMethod]
        public async Task LoginReturnsUnAuthorized_WithJwt_WhenCredsInvalid()
        {
            JwtResult jwtMock = new JwtResult() { Token = "MockToken" };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);

            publisherMock
                .Setup(m => m.Publish<JwtResult>(It.IsAny<LogGebruikerInCommand>(),
                    NameConstants.LogGebuikerInCommandQueue, "")).ThrowsAsync(new LoginFailedException("exception"));

            var target = new AuthenticatieController(publisherMock.Object);

            var result = await target.Login(new Credentials() { UserName = "email@.live.nl", Password = "password" });

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

        }
    }
}
