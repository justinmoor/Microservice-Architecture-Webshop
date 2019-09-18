using System.Collections.Generic;
using System.Threading.Tasks;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public interface IArtikelDatamapper
    {
        void Insert(Artikel artikel);
        Task<List<Artikel>> GetAll();

        void ChangeVoorraad(int artikelId, int newVoorraad);
    }
}