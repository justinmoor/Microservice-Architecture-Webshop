using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.WebScale.Commands;
using Moq;
using Sprinters.SharedTypes.BeheerService.Commands;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.Controllers;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class BestellingenControllerTest
    {
        [TestMethod]
        public async Task NieuweBestellingReturnsNieuweBestellingId()
        {
            var bestelling = new Bestelling
            {
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                KlantId = "1",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 5)
                }
            };

            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock.Setup(m => m.Publish<int>(It.Is<NieuweBestellingCommand>(c => c.KlantId == "1"),
                    NameConstants.NieuweBestellingCommandQueue, ""))
                .ReturnsAsync(1).Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);

            var controller = new BestellingenController(publisherMock.Object, datamapperMock.Object);

            var result = await controller.NieuweBestelling(bestelling);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task VolgendeBestellingReturnsVolgendeBestellingObject()
        {
            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock.Setup(m => m.Publish<int>(It.IsAny<VolgendeBestellingCommand>(),
                    NameConstants.VolgendeBestellingCommandQueue, ""))
                .ReturnsAsync(1).Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(d => d.Get(1)).ReturnsAsync(new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 5)
                }
            }).Verifiable();

            var controller = new BestellingenController(publisherMock.Object, datamapperMock.Object);

            var result = await controller.VolgendeBestelling();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var bestelling = (Bestelling) okResult.Value;

            Assert.AreEqual(1, bestelling.Id);
            Assert.AreEqual("1", bestelling.KlantId);
        }

        [TestMethod]
        public async Task VolgendeBestellingReturnsNotFoundWhenNoBestellingLeft()
        {
            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock.Setup(m => m.Publish<int>(It.IsAny<VolgendeBestellingCommand>(),
                    NameConstants.VolgendeBestellingCommandQueue, ""))
                .ReturnsAsync(0).Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);


            var controller = new BestellingenController(publisherMock.Object, datamapperMock.Object);

            var result = await controller.VolgendeBestelling();

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task FinishBestellingFinishesBestellingTest()
        {
            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock.Setup(m => m.Publish<int>(It.Is<BestellingAfrondenCommand>(b => b.Id == 1),
                    NameConstants.FinishBestellingCommandQueue, ""))
                .ReturnsAsync(1).Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);


            var controller = new BestellingenController(publisherMock.Object, datamapperMock.Object);

            var result = await controller.FinishBestelling(1);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task DatamapperGetsCalledWhenGettingBestellingenBoven500()
        {
            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(dm => dm.GetBestellingenBoven500()).ReturnsAsync(new List<Bestelling>()).Verifiable();

            var controller = new BestellingenController(null, datamapperMock.Object);

            await controller.GetBestellingenBoven500Eu();
        }

        [TestMethod]
        public async Task BestellingGoedkeurenTest()
        {
            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock.Setup(p => p.Publish<int>(It.Is<BestellingGoedkeurenCommand>(b => b.Id == 1),
                    NameConstants.BestellingGoedKeurenCommandQueue, ""))
                .ReturnsAsync(1).Verifiable();

            var controller = new BestellingenController(publisherMock.Object, null);

            var result = await controller.BestellingGoedkeuren(1) as OkObjectResult;

            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public async Task BestellingAfkeurenTest()
        {
            var bestellingAfkeurenResult = new BestellingAfkeurenResult()
            {
                Id = 1,
                IsSuccesfull = true,
                PassedBestellingen = new List<int>() {5, 8, 10}
            };
            
            var publisherMock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            publisherMock.Setup(p => p.Publish<BestellingAfkeurenResult>(It.Is<BestellingAfkeurenCommand>(b => b.Id == 1),
                    NameConstants.BestellingAfkeurenCommandQueue, ""))
                .ReturnsAsync(bestellingAfkeurenResult).Verifiable();

            var controller = new BestellingenController(publisherMock.Object, null);

            var result = await controller.BestellingAfkeuren(1) as OkObjectResult;

            List<int> resultList = result.Value as List<int>;

            Assert.IsNotNull(resultList);
            Assert.AreEqual(3, resultList.Count);
        }
    }
}