using System.Collections.Generic;
using System.Threading.Tasks;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public interface IKlantDatamapper
    {
        void Insert(Klant artikel);
        Task<Klant> GetKlant(string id);
        Task Update(Klant klant);

        Task<List<Bestelling>> GetUnFinishedBestellingenOfKlant(string klantId);
        Task<Klant> GetKlantWithBestellingId(int bestellingId);
    }
}