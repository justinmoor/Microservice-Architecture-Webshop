using System.Collections.Generic;
using System.Threading.Tasks;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.DAL
{
    public interface IBestellingDatamapper
    {
        Task<Bestelling> Get(int id);
        Task Insert(Bestelling bestelling);

        Task<IEnumerable<Bestelling>> GetBestellingenBoven500();

        Task Update(Bestelling bestelling);
    }
}