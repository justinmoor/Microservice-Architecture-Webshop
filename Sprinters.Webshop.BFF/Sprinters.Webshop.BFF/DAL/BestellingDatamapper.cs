using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprinters.SharedTypes.BeheerService.Entities;
using Bestelling = Sprinters.Webshop.BFF.Entities.Bestelling;

namespace Sprinters.Webshop.BFF.DAL
{
    public class BestellingDatamapper : IBestellingDatamapper
    {
        private readonly WebshopContext _context;

        public BestellingDatamapper(WebshopContext context)
        {
            _context = context;
        }

        public async Task Insert(Bestelling bestelling)
        {
            _context.Bestellingen.Add(bestelling);
            await _context.SaveChangesAsync();
        }

        public async Task<Bestelling> Get(int id)
        {
            return await _context.Bestellingen.Include(b => b.Klant).Include(b => b.BesteldeArtikelen)
                .ThenInclude(a => a.Artikel)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bestelling>> GetBestellingenBoven500()
        {
            return await _context.Bestellingen
                .Include(b => b.BesteldeArtikelen)
                .ThenInclude(a => a.Artikel)
                .Include(k => k.Klant)
                .Where(b => b.BestellingStatus == BestellingStatus.TerControleVoorSales)
				.Orderby(b => b.Id)
                .ToListAsync();
        }

        public async Task Update(Bestelling bestelling)
        {
            _context.Bestellingen.Update(bestelling);
            await _context.SaveChangesAsync();
        }
    }
}