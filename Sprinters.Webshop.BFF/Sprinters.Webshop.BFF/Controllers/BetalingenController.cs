using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minor.Nijn.WebScale.Commands;
using Sprinters.SharedTypes.BetaalService;
using Sprinters.SharedTypes.Shared;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetalingenController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        public BetalingenController(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> BetaalBestelling(Betaling betaling)
        {
            var command = new BetaalBestellingCommand
            {
                Bedrag = betaling.Bedrag,
                BestellingId = betaling.Factuurnummer
            };
            var bestellingId =
                await _commandPublisher.Publish<int>(command, NameConstants.BetaalBestellingCommandQueue);

            if (bestellingId == 0) return BadRequest();

            return Ok();
        }
    }
}