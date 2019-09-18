using System.Threading.Tasks;
using Sprinters.SharedTypes.KlantService.Entities;

namespace Sprinters.KlantBeheer.DAL
{
    public interface IKlantDatamapper
    {
        Task Insert(Klant klant);
    }
}