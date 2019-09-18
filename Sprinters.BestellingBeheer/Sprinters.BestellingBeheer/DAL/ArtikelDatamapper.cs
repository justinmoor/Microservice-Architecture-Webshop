using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public class ArtikelDatamapper : IArtikelDatamapper
    {
        private readonly BeheerContext _context;

        public ArtikelDatamapper(BeheerContext context)
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

        //Not async because audit scrape has to be called sync
        public void ChangeVoorraad(int artikelId, int newVoorraad)
        {
            var artikel = _context.Artikelen.FirstOrDefault(a => a.Artikelnummer == artikelId);
            artikel.Voorraad = newVoorraad;
            _context.SaveChanges();
        }
    }
}