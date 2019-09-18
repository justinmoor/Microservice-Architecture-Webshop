using System.Threading.Tasks;
using Sprinters.SharedTypes.KlantService.Entities;

namespace Sprinters.KlantBeheer.DAL
{
    public class KlantDatamapper : IKlantDatamapper
    {
        private readonly KlantContext _context;

        public KlantDatamapper(KlantContext context)
        {
            _context = context;
        }

        public async Task Insert(Klant klant)
        {
            _context.Klanten.Add(klant);
            await _context.SaveChangesAsync();
        }
    }
}