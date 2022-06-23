using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public class Recipient
    {
        public int Id { get; set; }
        public string Who { get; set; }
        public string Where { get; set; }
        public string Index { get; set; }
        public string Group { get; set; }

    }
}
