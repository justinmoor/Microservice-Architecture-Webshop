using Minor.Nijn.WebScale.Commands;
using Sprinters.SharedTypes.Authenticatie.Entities;

namespace Sprinters.SharedTypes.Authenticatie.Commands
{
    public class LogGebruikerInCommand : DomainCommand
    {
        public Credentials Credentials { get; set; }
    }
}