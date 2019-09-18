using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.KlantService.Commands
{
    public class RegistreerKlantCommand : DomainCommand
    {
        public string AccountId { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Telefoonnummer { get; set; }
        public string AdresRegel { get; set; }
        public string Plaats { get; set; }
        public string Postcode { get; set; }
    }
}