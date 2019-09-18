using System.Collections.Generic;
using System.Threading.Tasks;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.DAL
{
    public interface IArtikelDatamapper
    {
        void Insert(Artikel artikel);
        Task<List<Artikel>> GetAll();

        void ChangeVoorraad(int artikelId, int newVoorraad);
        Task<Artikel> Get(int id);
    }
}