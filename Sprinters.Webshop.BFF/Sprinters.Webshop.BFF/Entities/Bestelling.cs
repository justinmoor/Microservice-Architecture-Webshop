using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.Webshop.BFF.Entities
{
    public class Bestelling
    {
        public int Id { get; set; }

        [StringLength(255, ErrorMessage = "Adresregel1 mag niet meer dan 255 karakters bevatten")]
        [Required(ErrorMessage = "AdresRegel1 is verplicht")]
        public string AdresRegel1 { get; set; }

        public string AdresRegel2 { get; set; }

        [StringLength(255, ErrorMessage = "Plaats mag niet meer dan 255 karakters bevatten")]
        [Required(ErrorMessage = "Plaats is verplicht")]
        public string Plaats { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [StringLength(7, ErrorMessage = "Postcode mag niet langer zijn dan 7 karakters")]
        public string Postcode { get; set; }

        public Klant Klant { get; set; }
        public string KlantId { get; set; }

        public BestellingStatus BestellingStatus { get; set; }

        public List<BestellingItem> BesteldeArtikelen { get; set; }
    }
}