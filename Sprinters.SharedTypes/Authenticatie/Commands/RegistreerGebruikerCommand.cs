using Minor.Nijn.WebScale.Commands;
using Sprinters.SharedTypes.Authenticatie.Entities;

namespace Sprinters.SharedTypes.Authenticatie.Commands
{
    public class RegistreerGebruikerCommand : DomainCommand
    {
        public Account NewUser { get; set; }
    }
}