using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.Authenticatie.Commands
{
    public class VerwijderGebruikerCommand : DomainCommand
    {
        public string Id { get; set; }
    }
}