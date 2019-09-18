using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.WebScale;
using Moq;
using Sprinters.KlantBeheer.DAL;
using Sprinters.KlantBeheer.Listeners;
using Sprinters.SharedTypes.KlantService.Commands;
using Sprinters.SharedTypes.KlantService.Entities;
using Sprinters.SharedTypes.KlantService.Events;
using Sprinters.SharedTypes.Shared;

namespace Sprinters.KlantBeheer.Test
{
    [TestClass]
    public class KlantListenerTest
    {
        [TestMethod]
        public async Task RegistreerKlantCommandInsertsKlantAndThrowsEvent()
        {
            var eventpublisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            eventpublisherMock.Setup(m =>
                m.Publish(
                    It.Is<KlantGeregistreerdEvent>(d => d.RoutingKey == NameConstants.KlantGeregistreerdEvent)));

            var datamapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            datamapperMock
                .Setup(m => m.Insert(It.Is<Klant>(b => b.Voornaam == "Hans" && b.Id == "1")))
                .Returns(Task.CompletedTask).Verifiable();

            var command = new RegistreerKlantCommand
            {
                AccountId = "1",
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                AdresRegel = "Voorstraat 8",
                Plaats = "Groningen",
                Postcode = "1345df",
                Telefoonnummer = "0665234365"
            };

            var listener = new KlantListener(datamapperMock.Object, eventpublisherMock.Object);

            var result = await listener.RegistreerKlant(command);

            Assert.AreEqual("1", result);
        }
    }
}