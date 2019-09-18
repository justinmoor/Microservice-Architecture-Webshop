using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Nijn.TestBus;
using Minor.Nijn.WebScale.Commands;
using Minor.Nijn.WebScale.Events;
using Sprinters.BestellingBeheer.DAL;
using Sprinters.BestellingBeheer.Listeners;
using Sprinters.SharedTypes.BeheerService.Commands;
using Sprinters.SharedTypes.BeheerService.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sprinters.BestellingBeheer.Spec
{
    [Binding]
    public class BestellingBeheerSteps
    {
        private Klant klant;
        private BestellingListener bestellingListener;
        private SqliteConnection connection;
        private DbContextOptions options;
        private NieuweBestellingCommand bestelling;
        private Artikel artikel;
        private int id;


        [Given(@"dat er een bestelling van ""(.*)"" is geplaatst")]
        public void GivenDatErEenBestellingVanIsGeplaatst(decimal totaalBedrag)
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            options = new DbContextOptionsBuilder<BeheerContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new BeheerContext(options))
            {
                context.Database.EnsureCreated();
            }
            artikel = new Artikel
            {
                Artikelnummer = 1,
                Beschrijving = "Test beschrijving",
                LeverbaarTot = new DateTime(2019, 01, 22),
                LeverbaarVanaf = new DateTime(2019, 01, 21),
                Naam = "Fiets",
                Prijs = (totaalBedrag / 1.21M),
                Voorraad = 10
            };

            using (var context = new BeheerContext(options))
            {
                ArtikelDatamapper artikelDatamapper = new ArtikelDatamapper(context);
                artikelDatamapper.Insert(artikel);
            }

            bestelling = new NieuweBestellingCommand
            {
                KlantId = "1",
                AdresRegel1 = "Laagstraat 11",
                Plaats = "Laaghoven",
                Postcode = "1234FG",
                BesteldeArtikelen = new List<BestellingItem>
                {
                    new BestellingItem(1, 1)
                }
            };
        }
        
        [Given(@"het Krediet van ""(.*)"" van de klant boven de limiet van ""(.*)"" is")]
        public void GivenHetKredietVanVanDeKlantBovenDeLimietVanIs(decimal kredietBedrag, decimal limietBedrag)
        {
            klant = new Klant {
                Id = "1",
                Voornaam = "Negin",
                Achternaam = "Nafissi",
                KredietMetSales = kredietBedrag
            };

            using (var context = new BeheerContext(options))
            {
                KlantDatamapper klantDatamapper = new KlantDatamapper(context);
                klantDatamapper.Insert(klant);
            }

        }
        
        [When(@"de bestelling is geplaatst")]
        public async Task WhenDeBestellingIsGeplaatst()
        {
            using (var context = new BeheerContext(options))
            {
                BestellingDatamapper bestellingDatamapper = new BestellingDatamapper(context);
                KlantDatamapper klantDatamapper = new KlantDatamapper(context);
                TestBusContext testBusContext = new TestBusContext();
                CommandPublisher commandPublisher = new CommandPublisher(testBusContext);
                EventPublisher eventPublisher = new EventPublisher(testBusContext);

                bestellingListener = new BestellingListener(bestellingDatamapper, klantDatamapper, commandPublisher,eventPublisher);

                id = await bestellingListener.PlaatsBestelling(bestelling);
            }
            
        }
        
        [Then(@"de bestelling naar sales wordt ""(.*)"" doorgestuurd")]
        public async Task ThenDeBestellingNaarSalesWordtDoorgestuurd(string toestand)
        {
            using (var context = new BeheerContext(options))
            {
                BestellingDatamapper bestellingDatamapper = new BestellingDatamapper(context);
                var bestaandeBestelling = await bestellingDatamapper.GetBestelling(id);
                if(toestand == "wel")
                {
                    Assert.IsTrue(bestaandeBestelling.BestellingStatus == BestellingStatus.TerControleVoorSales);
                }
                else
                {
                    Assert.IsTrue(bestaandeBestelling.BestellingStatus == BestellingStatus.GereedVoorBehandeling);
                }
                
            }
        }
    }
}
