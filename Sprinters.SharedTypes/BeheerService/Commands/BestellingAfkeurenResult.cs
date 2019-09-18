using System;
using System.Collections.Generic;
using System.Text;

namespace Sprinters.SharedTypes.BeheerService.Commands
{
    public class BestellingAfkeurenResult
    {

        public int Id { get; set; }
        public bool IsSuccesfull { get; set; }

        public List<int> PassedBestellingen { get; set; }
    }
}
