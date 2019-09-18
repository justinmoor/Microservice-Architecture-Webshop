using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.DAL
{
    public class ArtikelDatamapper : IArtikelDatamapper
    {
        private readonly WebshopContext _context;

        public ArtikelDatamapper(WebshopContext context)
        {
            _context = context;
        }

        public void Insert(Artikel artikel)
        {
            _context.Artikelen.Add(artikel);
            _context.SaveChanges();
        }

        public async Task<List<Artikel>> GetAll()
        {
            return await _context.Artikelen.ToListAsync();
        }

        public void ChangeVoorraad(int artikelId, int newVoorraad)
        {
            var artikel = _context.Artikelen.FirstOrDefault(a => a.Artikelnummer == artikelId);
            artikel.Voorraad = newVoorraad;
            _context.SaveChanges();
        }

        public async Task<Artikel> Get(int id)
        {
            return await _context.Artikelen.FirstOrDefaultAsync(a => a.Artikelnummer == id);
        }
    }
}