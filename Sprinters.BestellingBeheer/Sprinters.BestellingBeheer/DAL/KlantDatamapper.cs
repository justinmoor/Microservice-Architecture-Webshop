using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public class KlantDatamapper : IKlantDatamapper
    {
        private readonly BeheerContext _context;

        public KlantDatamapper(BeheerContext context)
        {
            _context = context;
        }

        public void Insert(Klant klant)
        {
            _context.Klanten.Add(klant);
            _context.SaveChanges();
        }

        public async Task Update(Klant klant)
        {
            _context.Klanten.Update(klant);
            await _context.SaveChangesAsync();
        }

        public async Task<Klant> GetKlant(string id)
        {
            return await _context.Klanten.FirstAsync(k => k.Id == id);
        }

        public async Task<Klant> GetKlantWithBestellingId(int bestellingId)
        {
            var bestelling = await _context.Bestellingen.Include(b => b.Klant)
                .FirstOrDefaultAsync(b => b.Id == bestellingId);
            return bestelling?.Klant;
        }

        public async Task<List<Bestelling>> GetUnFinishedBestellingenOfKlant(string klantId)
        {
            return await _context.Bestellingen.Include(b => b.BesteldeArtikelen).ThenInclude(b => b.Artikel).Where(b =>
                b.KlantId == klantId && b.BestellingStatus == BestellingStatus.TerControleVoorSales).ToListAsync();
        }
    }
}