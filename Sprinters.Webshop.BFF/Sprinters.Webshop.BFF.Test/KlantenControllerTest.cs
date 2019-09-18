using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sprinters.Webshop.BFF.Controllers;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Test
{
    [TestClass]
    public class KlantenControllerTest
    {

        [TestMethod]
        public async Task GetKlantReturnsKlant()
        {
            var klant = new Klant
            {
                Id = "1",
                Voornaam = "Hans",
                Achternaam = "van Huizen",
                AdresRegel = "Voorstraat 8",
                Plaats = "Groningen",
                Postcode = "1345df",
                Telefoonnummer = "0665234365"
            };

            var klantmapperMock = new Mock<IKlantDatamapper>(MockBehavior.Strict);
            klantmapperMock.Setup(k => k.GetKlant("1")).ReturnsAsync(klant);

            var controller = new KlantenController(klantmapperMock.Object);

            var result = await controller.GetKlant("1");

            Assert.AreEqual("Hans", result.Voornaam);
            Assert.AreEqual("van Huizen", result.Achternaam);
            Assert.AreEqual("1", result.Id);
        }
    }
}
