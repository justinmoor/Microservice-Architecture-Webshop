using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minor.Nijn.WebScale.Commands;
using Sprinters.Webshop.BFF.DAL;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtikelenController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly IArtikelDatamapper _datamapper;


        public ArtikelenController(IArtikelDatamapper datamapper)
        {
            _datamapper = datamapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Artikel>>> GetArtikelen()
        {
            return await _datamapper.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Artikel> GetArtikel([FromRoute] int id)
        {
            return await _datamapper.Get(id);
        }
    }
}