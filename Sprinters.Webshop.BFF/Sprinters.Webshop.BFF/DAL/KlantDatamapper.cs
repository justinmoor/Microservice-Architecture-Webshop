using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.DAL
{
    public class KlantDatamapper : IKlantDatamapper
    {
        private readonly WebshopContext _context;

        public KlantDatamapper(WebshopContext context)
        {
            _context = context;
        }

        public void Insert(Klant klant)
        {
            _context.Klanten.Add(klant);
            _context.SaveChanges();
        }

        public void Update(Klant klant)
        {
            _context.Klanten.Update(klant);
            _context.SaveChanges();
        }

        public async Task<Klant> GetKlant(string id)
        {
            return await _context.Klanten.FirstAsync(k => k.Id == id);
        }
    }
}