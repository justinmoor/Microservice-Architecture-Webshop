using System.ComponentModel.DataAnnotations;

namespace Sprinters.Webshop.BFF.Entities
{
    public class Categorie
    {
        [Key] public int Id { get; set; }

        public string Naam { get; set; }
    }
}