using System.Threading.Tasks;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public interface IBestellingDatamapper
    {
        Task Insert(Bestelling bestelling);
        Task<Bestelling> GetBestelling(int id);
        Task<int> GetFirstUndone();
        Task Update(Bestelling bestelling);
    }
}