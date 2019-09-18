using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.BestellingBeheer.Listeners;
using Sprinters.SharedTypes.BeheerService.Entities;
using Sprinters.SharedTypes.KlantService.Events;

namespace Sprinters.BestellingBeheer.Test
{
    [TestClass]
    public class KlantListenerTest
    {
        [TestMethod]
        public void KlantGeregistreerdEventAddsToDatabase()
        {
            var artikelEvent = new KlantGeregistreerdEvent
            {
                Id = "1",
                Voornaam = "Hans",
                Achternaam = "Van Huizen"
            };

            var mapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.Insert(It.Is<Klant>(a => a.Id == "1"))).Verifiable();
            var magazijnListener = new KlantListener(mapperMock.Object);

            magazijnListener.KlantToegevoegd(artikelEvent);
        }
    }
}