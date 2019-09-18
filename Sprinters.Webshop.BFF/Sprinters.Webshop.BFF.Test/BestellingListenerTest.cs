using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.BeheerService.Events;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Listeners;
using Bestelling = Sprinters.Webshop.BFF.Entities.Bestelling;
using BestellingItem = Sprinters.Webshop.BFF.Entities.BestellingItem;
using BestellingToegevoegdEvent = Sprinters.Webshop.BFF.Entities.BestellingToegevoegdEvent;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class BestellingListenerTest
    {
        [TestMethod]
        public void BestellingToegevoegd_InsertGetsCalled()
        {
            var bestelling = new BestellingToegevoegdEvent()
            {
                Bestelling = new Bestelling()
                {
                    Id = 1,
                    KlantId = "1",
                    AdresRegel1 = "Laagstraat 11",
                    Plaats = "Laaghoven",
                    Postcode = "1234FG",
                    BesteldeArtikelen = new List<BestellingItem>
                    {
                        new BestellingItem(1, 3) {Id = 1},
                        new BestellingItem(2, 5) {Id = 2}
                    }
                }
            };

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(dm => dm.Insert(It.Is<Bestelling>(b => b.Id == 1))).Returns(Task.CompletedTask).Verifiable();

            var bestellingListener = new BestellingListener(datamapperMock.Object);
            bestellingListener.BestellingToegevoegd(bestelling);

            datamapperMock.Verify(dm => dm.Insert(It.Is<Bestelling>(b => b.Id == 1)));
        }


        [TestMethod]
        public async Task BestellingVerzonden_ChangesBestellingStatus()
        {
            var bestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.InBehandelingDoorMagazijn,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Id = 1},
                    new BestellingItem(2, 5) {Id = 2}
                }
            };

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(dm => dm.Get(bestelling.Id))
                .ReturnsAsync(bestelling)
                .Verifiable();

            datamapperMock.Setup(dm => dm.Update(It.Is<Bestelling>(b => b.Id == 1))).Returns(Task.CompletedTask)
                .Verifiable();

            var listener = new BestellingListener(datamapperMock.Object);

            await listener.BestellingVerzonden(new BestellingVerzondenEvent {Id = bestelling.Id});

            Assert.AreEqual(BestellingStatus.Verzonden, bestelling.BestellingStatus);
        }

        [TestMethod]
        public async Task BestellingInpakkenGestart_ChangesBestellingStatus()
        {
            var bestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.GereedVoorBehandeling,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Id = 1},
                    new BestellingItem(2, 5) {Id = 2}
                }
            };

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(dm => dm.Get(bestelling.Id))
                .ReturnsAsync(bestelling)
                .Verifiable();

            datamapperMock.Setup(dm => dm.Update(It.Is<Bestelling>(b => b.Id == 1))).Returns(Task.CompletedTask)
                .Verifiable();

            var listener = new BestellingListener(datamapperMock.Object);

            await listener.BestellingInpakkenGestart(new BestellingInpakkenGestartEvent {Id = bestelling.Id});

            Assert.AreEqual(BestellingStatus.InBehandelingDoorMagazijn, bestelling.BestellingStatus);
        }

        [TestMethod]
        public async Task BestellingGoedGekeurd_ChangesStatus()
        {
            var bestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Id = 1},
                    new BestellingItem(2, 5) {Id = 2}
                }
            };

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(dm => dm.Get(1))
                .ReturnsAsync(bestelling)
                .Verifiable();

            datamapperMock.Setup(dm => dm.Update(It.Is<Bestelling>(b => b.Id == 1))).Returns(Task.CompletedTask)
                .Verifiable();

            var listener = new BestellingListener(datamapperMock.Object);

            await listener.BestellingGoedgekeurd(new BestellingGoedGekeurdEvent {Id = bestelling.Id});

            Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, bestelling.BestellingStatus);
        }

        [TestMethod]
        public async Task BestellingAfgekeurd_ChangesStatus()
        {
            var bestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Id = 1},
                    new BestellingItem(2, 5) {Id = 2}
                }
            };

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock.Setup(dm => dm.Get(bestelling.Id))
                .ReturnsAsync(bestelling)
                .Verifiable();

            datamapperMock.Setup(dm => dm.Update(It.Is<Bestelling>(b => b.Id == 1))).Returns(Task.CompletedTask)
                .Verifiable();

            var listener = new BestellingListener(datamapperMock.Object);

            await listener.BestellingAfgekeurd(new BestellingAfgekeurdEvent {Id = bestelling.Id});

            Assert.AreEqual(BestellingStatus.Afgekeurd, bestelling.BestellingStatus);
        }
    }
}