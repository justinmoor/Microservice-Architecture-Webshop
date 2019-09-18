using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public class BestellingDatamapper : IBestellingDatamapper
    {
        private readonly BeheerContext _context;

        public BestellingDatamapper(BeheerContext context)
        {
            _context = context;
        }

        public async Task Insert(Bestelling bestelling)
        {
            _context.Bestellingen.Add(bestelling);
            await _context.SaveChangesAsync();
        }

        public async Task<Bestelling> GetBestelling(int id)
        {
            return await _context.Bestellingen.Include(b => b.Klant).Include(b => b.BesteldeArtikelen)
                .ThenInclude(a => a.Artikel)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> GetFirstUndone()
        {
            var bestelling = await _context.Bestellingen
                .Where(b => b.BestellingStatus == BestellingStatus.GereedVoorBehandeling)
                .OrderBy(b => b.Id)
                .FirstOrDefaultAsync();

            if (bestelling == null) return 0;

            bestelling.BestellingStatus = BestellingStatus.InBehandelingDoorMagazijn;
            await _context.SaveChangesAsync();
            return bestelling.Id;
        }


        public async Task Update(Bestelling bestelling)
        {
            _context.Bestellingen.Update(bestelling);
            await _context.SaveChangesAsync();
        }
    }
}