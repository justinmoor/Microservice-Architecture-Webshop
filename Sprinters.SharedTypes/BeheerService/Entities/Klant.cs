using System.ComponentModel.DataAnnotations;

namespace Sprinters.SharedTypes.BeheerService.Entities
{
    public class Klant
    {
        [Key] public string Id { get; set; }

        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public decimal KredietOver { get; set; }
        public decimal KredietMetSales { get; set; }
    }
}