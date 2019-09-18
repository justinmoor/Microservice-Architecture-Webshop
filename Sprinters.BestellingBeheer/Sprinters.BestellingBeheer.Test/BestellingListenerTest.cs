using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.WebScale;
using Minor.Nijn.WebScale.Commands;
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
    public class BestellingListenerTest
    {
        [TestMethod]
        public async Task NewBestellingUnderKredietLimitEuroInsertsIntoDatabaseWithStatusGereed()
        {
            var bestelling = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 5)
                }
            };

            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<BestellingToegevoegdEvent>(d => d.RoutingKey == NameConstants.BestellingToegevoegdEvent)));
            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<KlantKredietAangepastEvent>(e =>
                        e.RoutingKey == NameConstants.KlantKredietAangepastEvent)))
                .Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.Insert(It.Is<Bestelling>(b => b.KlantId == "1" && b.BesteldeArtikelen.Count == 2)))
                .Callback((Bestelling b) => b.Id = 1)
                .Returns(Task.CompletedTask).Verifiable();

            var klant = new Klant() {Id = "1", KredietOver = 500m};

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
                    new BestellingItem(1, 3) {Artikel = new Artikel {Prijs = 19.99m}},
                    new BestellingItem(2, 5) {Artikel = new Artikel {Prijs = 14.99m}}
                }
            };

            datamapperMock.Setup(m => m.GetBestelling(1)).ReturnsAsync(finalBestelling).Verifiable();

            datamapperMock
                .Setup(m => m.Update(
                    It.Is<Bestelling>(b => b.BestellingStatus == BestellingStatus.GereedVoorBehandeling)))
                .Returns(Task.CompletedTask)
                .Verifiable("Bestelling should have GereedVoorBehandeling status");

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            klantDatamapperMock.Setup( kd => kd.Update(It.Is<Klant>(k => k.Id == "1"))).Returns(Task.CompletedTask).Verifiable();

            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, eventpublisherMock.Object);

            var result = await listener.PlaatsBestelling(bestelling);

            Assert.AreEqual(1, result);
            Assert.AreEqual(163.27m, klant.KredietMetSales);
            Assert.AreEqual(336.73m, klant.KredietOver);
        }

        [TestMethod]
        public async Task NewBestellingAboveKredietLimitEuroInsertsIntoDatabaseWithStatusControleVoorSales()
        {
            var bestelling = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Artikel = new Artikel {Artikelnummer = 1, Prijs = 344.99m}},
                    new BestellingItem(2, 5) {Artikel = new Artikel {Artikelnummer = 2, Prijs = 233.49m}}
                }
            };

            var finalBestelling = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                Klant = new Klant() { Id = "1" },
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3) {Artikel = new Artikel {Artikelnummer = 1, Prijs = 344.99m}},
                    new BestellingItem(2, 5) {Artikel = new Artikel {Artikelnummer = 2, Prijs = 233.49m}}
                }
            };


            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<BestellingToegevoegdEvent>(d => d.RoutingKey == NameConstants.BestellingToegevoegdEvent)));
            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<KlantKredietAangepastEvent>(e =>
                        e.RoutingKey == NameConstants.KlantKredietAangepastEvent)))
                .Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.Insert(It.Is<Bestelling>(b =>
                    b.KlantId == "1" && b.BesteldeArtikelen.Count == 2 &&
                    b.BestellingStatus == BestellingStatus.TerControleVoorSales)))
                .Callback((Bestelling b) => b.Id = 1)
                .Returns(Task.CompletedTask)
                .Verifiable("Bestelling needs to have ControlevoorSales status");

            datamapperMock.Setup(m => m.GetBestelling(1)).ReturnsAsync(finalBestelling);
            
            datamapperMock
                .Setup(m => m.Update(
                    It.Is<Bestelling>(b => b.BestellingStatus == BestellingStatus.TerControleVoorSales)))
                .Returns(Task.CompletedTask)
                .Verifiable("Bestelling should have GereedVoorBehandeling status");

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            klantDatamapperMock.Setup(kd => kd.Update(It.Is<Klant>(k => k.Id == "1"))).Returns(Task.CompletedTask).Verifiable();

            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, eventpublisherMock.Object);

            var result = await listener.PlaatsBestelling(bestelling);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task MultipleBestellingenInsertIntoDatabaseWithStatusGereedvoorBehandelingUntilKredietLimietIsBereikt()
        {
            var klant = new Klant() {Id = "1", KredietOver = 500} ;

            var bestelling1 = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1),
                }
            };
            var bestelling2 = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1),
                }
            };
            var bestelling3 = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1),
                }
            };

            var finalBestelling1 = new Bestelling
            {
                Id = 1,
                KlantId = "1",
                Klant = klant,
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Artikel = new Artikel {Prijs = 19.99m}},
                }
            };
            var finalBestelling2 = new Bestelling
            {
                Id = 2,
                KlantId = "1",
                Klant = klant,
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Artikel = new Artikel {Prijs = 199.99m}},
                }
            };
            var finalBestelling3 = new Bestelling
            {
                Id = 3,
                KlantId = "1",
                Klant = klant,
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Artikel = new Artikel {Prijs = 249.99m}},
                }
            };

            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<BestellingToegevoegdEvent>(d => d.RoutingKey == NameConstants.BestellingToegevoegdEvent)));

            int id = 0;
            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.Insert(It.Is<Bestelling>(b =>
                    b.KlantId == "1")))
                .Callback((Bestelling b) => b.Id = ++id )
                .Returns(Task.CompletedTask)
                .Verifiable("Bestelling needs to have ControlevoorSales status");

            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<KlantKredietAangepastEvent>(e =>
                        e.RoutingKey == NameConstants.KlantKredietAangepastEvent)))
                .Verifiable();


            datamapperMock.Setup(m => m.GetBestelling(1)).ReturnsAsync(finalBestelling1);
            datamapperMock.Setup(m => m.GetBestelling(2)).ReturnsAsync(finalBestelling2);
            datamapperMock.Setup(m => m.GetBestelling(3)).ReturnsAsync(finalBestelling3);

            datamapperMock
                .Setup(m => m.Update(
                    It.Is<Bestelling>(b => 
                         (b.BestellingStatus == BestellingStatus.GereedVoorBehandeling && (b.Id == 1 || b.Id == 2)) ||
                         (b.BestellingStatus == BestellingStatus.TerControleVoorSales && b.Id == 3))))
                .Returns(Task.CompletedTask)
                .Verifiable("Bestelling should have GereedVoorBehandeling status");

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);

            klantDatamapperMock.Setup(k => k.Update(It.Is<Klant>(kl => kl.Id == "1"))).Returns(Task.CompletedTask).Verifiable();


            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, eventpublisherMock.Object);

            var result1 = await listener.PlaatsBestelling(bestelling1);
            var result2 = await listener.PlaatsBestelling(bestelling2);
            var result3 = await listener.PlaatsBestelling(bestelling3);

            Assert.AreEqual(1, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(3, result3);
            Assert.AreEqual(568.67m, klant.KredietMetSales);
            Assert.AreEqual(233.82m, klant.KredietOver);

            //2 bestellingen will go to magazijn, 1 not so check for 2 times
            klantDatamapperMock.Verify(k => k.Update(It.Is<Klant>(kl => kl.Id == "1")), Times.Exactly(3));
        }

        [TestMethod]
        public async Task VolgendeBestellingGivesVolgendeBestellingAndThrowsInpakkenGestartEvent()
        {
            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<BestellingInpakkenGestartEvent>(d =>
                        d.RoutingKey == NameConstants.BestellingInpakkenGestartEvent)));

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.GetFirstUndone()).ReturnsAsync(1);

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);

            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, eventpublisherMock.Object);

            int? result = await listener.VolgendeBestellingInpakken(new VolgendeBestellingCommand());

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task VolgendeBestellingWhenNoMoreLeftReturns0()
        {
            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.GetFirstUndone()).ReturnsAsync(0);

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);

            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, null);
            var result = await listener.VolgendeBestellingInpakken(new VolgendeBestellingCommand());

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task FinishBestellingLowersVoorraad()
        {
            var bestelling = new Bestelling
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 3),
                    new BestellingItem(2, 5)
                }
            };

            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<BestellingVerzondenEvent>(d => d.RoutingKey == NameConstants.BestellingVerzondenEvent)));

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.GetBestelling(1))
                .ReturnsAsync(bestelling);

            var mock = new Mock<ICommandPublisher>(MockBehavior.Strict);
            mock.Setup(s => s.Publish<bool>(It.IsAny<HaalVoorraadUitMagazijnCommand>(), "Kantilever.MagazijnService",
                "Kantilever.MagazijnService.HaalVoorraadUitMagazijnCommand")).ReturnsAsync(false);

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);

            var listener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, mock.Object, eventpublisherMock.Object);

            await listener.FinishBestelling(new BestellingAfrondenCommand {Id = 1});

            mock.Verify(
                s => s.Publish<bool>(It.IsAny<HaalVoorraadUitMagazijnCommand>(), "Kantilever.MagazijnService",
                    "Kantilever.MagazijnService.HaalVoorraadUitMagazijnCommand"), Times.Exactly(2));
        }

        [TestMethod]
        public async Task KeurBestellingGoed_UpdatesBestelling_And_FiresEvent_AndLowersKredietLeft()
        {
            var klant = new Klant() {Id = "1", KredietOver = 500m} ;

            var bestelling = new Bestelling
            {
                Id = 2,
                KlantId = "1",
                Klant = klant,
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Artikel = new Artikel {Prijs = 10m}},
                    new BestellingItem(2, 2) {Artikel = new Artikel {Prijs = 5m}}
                }
            };

            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<BestellingGoedGekeurdEvent>(e =>
                        e.RoutingKey == NameConstants.BestellingGoedgekeurdEvent)))
                .Verifiable();
            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<KlantKredietAangepastEvent>(e =>
                        e.RoutingKey == NameConstants.KlantKredietAangepastEvent)))
                .Verifiable();


            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);

            datamapperMock
                .Setup(m => m.GetBestelling(bestelling.Id))
                .ReturnsAsync(bestelling)
                .Verifiable();

            datamapperMock
                .Setup(m => m.Update(bestelling))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);

            klantDatamapperMock.Setup(k => k.Update(klant)).Returns(Task.CompletedTask).Verifiable();

            var bestellingListener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, eventpublisherMock.Object);

            var result = await bestellingListener.KeurBestellingGoed(new BestellingGoedkeurenCommand {Id = bestelling.Id});

            Assert.AreEqual(BestellingStatus.GereedVoorBehandeling, bestelling.BestellingStatus);
            Assert.AreEqual(475.80m, klant.KredietOver);
        }

        [TestMethod]
        public async Task KeurBestellingAf_UpdatesBestelling_And_FiresEvent()
        {
            var klant = new Klant()
            {
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                Id = "1",
                KredietMetSales = 30.25m,
                KredietOver = 469.75m
                
            };

            var bestelling = new Bestelling
            {
                Id = 2,
                KlantId = "1",
                Klant =  klant,
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BestellingStatus = BestellingStatus.TerControleVoorSales,
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1) {Artikel = new Artikel(){Artikelnummer = 1, Prijs = 20m}},
                    new BestellingItem(2, 1) {Artikel = new Artikel(){Artikelnummer = 1, Prijs = 5m}}
                }
            };

            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<BestellingAfgekeurdEvent>(e =>
                        e.RoutingKey == NameConstants.BestellingAfgekeurdEvent)))
                .Verifiable();

            eventpublisherMock
                .Setup(ep =>
                    ep.Publish(It.Is<KlantKredietAangepastEvent>(e =>
                        e.RoutingKey == NameConstants.KlantKredietAangepastEvent)))
                .Verifiable();

            var datamapperMock = new Mock<IBestellingDatamapper>(MockBehavior.Strict);

            datamapperMock
                .Setup(m => m.GetBestelling(bestelling.Id))
                .ReturnsAsync(bestelling)
                .Verifiable();

            datamapperMock
                .Setup(m => m.Update(bestelling))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var klantDatamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            klantDatamapperMock.Setup(k => k.GetUnFinishedBestellingenOfKlant("1"))
                .ReturnsAsync(new List<Bestelling>());
            klantDatamapperMock.Setup(k => k.Update(klant)).Returns(Task.CompletedTask).Verifiable();


            var bestellingListener = new BestellingListener(datamapperMock.Object, klantDatamapperMock.Object, null, eventpublisherMock.Object);

            var result = await bestellingListener.KeurBestellingAf(new BestellingAfkeurenCommand {Id = bestelling.Id});

            Assert.AreEqual(BestellingStatus.Afgekeurd, bestelling.BestellingStatus);
            Assert.AreEqual(0, bestelling.Klant.KredietMetSales);
            Assert.AreEqual(500, bestelling.Klant.KredietOver);
        }
    }
}