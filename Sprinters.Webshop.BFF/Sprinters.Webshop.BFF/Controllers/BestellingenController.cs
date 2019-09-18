using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minor.Nijn;
using Minor.Nijn.WebScale.Commands;
using Sprinters.SharedTypes.BeheerService.Commands;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;
using BestellingItemDB = Sprinters.SharedTypes.BeheerService.Entities.BestellingItem;

namespace Sprinters.Webshop.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingenController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly IBestellingDatamapper _datamapper;
        private readonly ILogger _logger;

        public BestellingenController(ICommandPublisher commandPublisher, IBestellingDatamapper datamapper)
        {
            _commandPublisher = commandPublisher;
            _datamapper = datamapper;
            _logger = NijnLogger.CreateLogger<BestellingenController>();
        }


        [HttpPost]
        public async Task<int> NieuweBestelling(Bestelling bestelling)
        {
            var besteldeArtikelen = new List<BestellingItemDB>();
            foreach (var bestellingItem in bestelling.BesteldeArtikelen)
                besteldeArtikelen.Add(new BestellingItemDB(bestellingItem.Artikelnummer, bestellingItem.Aantal));

            var command = new NieuweBestellingCommand
            {
                KlantId = bestelling.KlantId,
                AdresRegel1 = bestelling.AdresRegel1,
                AdresRegel2 = bestelling.AdresRegel2,
                BesteldeArtikelen = besteldeArtikelen,
                Plaats = bestelling.Plaats,
                Postcode = bestelling.Postcode
            };

            var result = await _commandPublisher.Publish<int>(command, NameConstants.NieuweBestellingCommandQueue);
            _logger.LogInformation("New bestelling with id {id} added", result);
            return result;
        }


        [HttpGet("next")]
        public async Task<IActionResult> VolgendeBestelling()
        {
            var result = await _commandPublisher.Publish<int>(new VolgendeBestellingCommand(),
                NameConstants.VolgendeBestellingCommandQueue);

            if (result == 0) return NotFound();

            var bestelling = await _datamapper.Get(result);

            _logger.LogInformation("Next bestelling {id}", result);


            return Ok(bestelling);
        }

        [HttpPost("finish/{id}")]
        public async Task<int> FinishBestelling([FromRoute] int id)
        {
            var result = await _commandPublisher.Publish<int>(new BestellingAfrondenCommand {Id = id},
                NameConstants.FinishBestellingCommandQueue);
            _logger.LogInformation("Finish bestelling {id}", id);

            return result;
        }

        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetBestellingenBoven500Eu()
        {
            var result = await _datamapper.GetBestellingenBoven500();

            return Ok(result);
        }

        [HttpPost("sales/goedkeuren/{id}")]
        public async Task<IActionResult> BestellingGoedkeuren([FromRoute] int id)
        {
            var result = await _commandPublisher.Publish<int>(new BestellingGoedkeurenCommand {Id = id},
                NameConstants.BestellingGoedKeurenCommandQueue);
            _logger.LogInformation("Approved Bestelling {id}", id);


            return Ok(result);
        }

        [HttpPost("sales/afkeuren/{id}")]
        public async Task<IActionResult> BestellingAfkeuren([FromRoute] int id)
        {
            var result = await _commandPublisher.Publish<BestellingAfkeurenResult>(new BestellingAfkeurenCommand {Id = id},
                NameConstants.BestellingAfkeurenCommandQueue);

            return Ok(result.PassedBestellingen);
        }
    }
}