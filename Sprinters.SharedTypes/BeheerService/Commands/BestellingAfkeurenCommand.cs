using Minor.Nijn.WebScale.Commands;

namespace Sprinters.SharedTypes.BeheerService.Commands
{
    public class BestellingAfkeurenCommand : DomainCommand
    {
        public int Id { get; set; }
    }
}