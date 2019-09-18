using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.WebScale;
using Moq;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.BestellingBeheer.Listeners;
using Sprinters.SharedTypes.BeheerService.Commands;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.BeheerService.Events;
using Sprinters.SharedTypes.BetaalService;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.BestellingBeheer.Test
{
    [TestClass]
    public class BetaalListenerTest
    {

        [TestMethod]
        public async Task BetaalBedragVerlaagdKredietEnKeurtBestellingAutomatischGoed()
        {

            var betaalCommand = new BetaalBestellingCommand()
            {
                BestellingId = 1,
                Bedrag = 100m
            };

            var klant = new Klant() {Id = "1", KredietMetSales = 100, KredietOver = 200};

            var finalBestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                Klant = klant,
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Artikel = new Artikel {Prijs = 20m}},
                    new BestellingItem(2, 1) {Artikel = new Artikel {Prijs = 5m}}
                }
            };

            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<BestellingGoedGekeurdEvent>(d => d.RoutingKey == NameConstants.BestellingGoedgekeurdEvent))).Verifiable();
            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<KlantKredietAangepastEvent>(e =>
                        e.RoutingKey == NameConstants.KlantKredietAangepastEvent)))
                .Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.Update(
                    It.Is<Bestelling>(b => b.BestellingStatus == BestellingStatus.GereedVoorBehandeling)))
                .Returns(Task.CompletedTask)
                .Verifiable("Bestelling should have GereedVoorBehandeling status");



            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            klantDatamapperMock.Setup(k => k.GetUnFinishedBestellingenOfKlant("1"))
                .ReturnsAsync(new List<Bestelling>() {finalBestelling})
                .Verifiable();

            klantDatamapperMock.Setup(k => k.GetKlantWithBestellingId(1)).ReturnsAsync(klant).Verifiable();
            klantDatamapperMock.Setup(k => k.Update(klant)).Returns(Task.CompletedTask).Verifiable();

            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object,null,
                eventpublisherMock.Object);

            var result = await listener.BetaalBestelling(betaalCommand);

            Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, finalBestelling.BestellingStatus);
            Assert.AreEqual(0, klant.KredietMetSales);
            Assert.AreEqual(269.75m, klant.KredietOver);
        }
    }
}
