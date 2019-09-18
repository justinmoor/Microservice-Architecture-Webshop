using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantenController : ControllerBase
    {
        private readonly IKlantDatamapper _datamapper;

        public KlantenController(IKlantDatamapper datamapper)
        {
            _datamapper = datamapper;
        }

        [HttpGet("{id}")]
        public async Task<Klant> GetKlant(string id)
        {
            var klant = await _datamapper.GetKlant(id);
            return klant;
        }
    }
}