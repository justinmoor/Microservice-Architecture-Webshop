using System.Threading.Tasks;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.DAL
{
    public interface IKlantDatamapper
    {
        Task<Klant> GetKlant(string id);
        void Insert(Klant klant);
        void Update(Klant klant);
    }
}