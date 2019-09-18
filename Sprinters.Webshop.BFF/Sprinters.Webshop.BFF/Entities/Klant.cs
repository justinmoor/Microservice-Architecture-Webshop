using System.ComponentModel.DataAnnotations;

namespace Sprinters.Webshop.BFF.Entities
{
    public class Klant
    {
        public string Id { get; set; }

        [StringLength(255, ErrorMessage = "Voornaam mag niet meer dan 255 karakters bevatten")]
        [Required(ErrorMessage = "Voornaam is vereist")]
        public string Voornaam { get; set; }

        [StringLength(255, ErrorMessage = "Achternaam mag niet meer dan 255 karakters bevatten")]
        [Required(ErrorMessage = "Achternaam is vereist")]
        public string Achternaam { get; set; }

        [StringLength(20, ErrorMessage = "Telefoonnummer mag niet meer dan 20 karakters bevatten")]
        [Required(ErrorMessage = "Telefoonnummer is vereist")]
        public string Telefoonnummer { get; set; }

        [StringLength(255, ErrorMessage = "Adresregel mag niet meer dan 255 karakters bevatten")]
        [Required(ErrorMessage = "AdresRegel is vereist")]
        public string AdresRegel { get; set; }

        [StringLength(255, ErrorMessage = "Plaats mag niet meer dan 255 karakters bevatten")]
        [Required(ErrorMessage = "Plaats is vereist")]
        public string Plaats { get; set; }

        [StringLength(7, ErrorMessage = "Postcode mag niet meer dan 7 karakters bevatten")]
        [Required(ErrorMessage = "Postcode is verplicht")]
        public string Postcode { get; set; }

        [EmailAddress(ErrorMessage = "Email Address is niet valide")]
        [Required(ErrorMessage = "Email is vereist")]
        public string Email { get; set; }

        //Deze wordt al gevalideerd in de authenticatie service er is voor gekozen om dit daar te laten
        public string Wachtwoord { get; set; }

        public decimal Krediet { get; set; }
    }
}