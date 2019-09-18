using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.WebScale.Commands;
using Moq;
using Sprinters.SharedTypes.BetaalService;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.Controllers;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class BetalingenControllerTest
    {

        [TestMethod]
        public async Task PlaatsBestellingTestReturnsOk()
        {


            var commandpublisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);

            commandpublisherMock.Setup(c => c.Publish<int>(It.Is<BetaalBestellingCommand>(b => b.BestellingId == 1 && b.Bedrag == 100m), NameConstants.BetaalBestellingCommandQueue, "")).ReturnsAsync(1).Verifiable();

            var controller = new BetalingenController(commandpublisherMock.Object);

            var betaling = new Betaling()
            {
                Bedrag = 100m,
                Factuurnummer = 1
            };

            var result = await controller.BetaalBestelling(betaling);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PlaatsNotExistingBestellingReturnsBadRequest()
        {


            var commandpublisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);

            commandpublisherMock.Setup(c => c.Publish<int>(It.Is<BetaalBestellingCommand>(b => b.BestellingId == 4059549 && b.Bedrag == 100m), NameConstants.BetaalBestellingCommandQueue, "")).ReturnsAsync(0).Verifiable();

            var controller = new BetalingenController(commandpublisherMock.Object);

            var betaling = new Betaling()
            {
                Bedrag = 100m,
                Factuurnummer = 4059549
            };

            var result = await controller.BetaalBestelling(betaling);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
