using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public class PrintSettings
    {
        public int Id { get; set; }
        public string EnvelopeFormat { get; set; }
        public double EnvelopeWidth { get; set; }
        public double EnvelopeHeight { get; set; }

        public double WhoFromSideSet { get; set; }
        public double WhoFromTopSet { get; set; }
        public double WhoFromWidth { get; set; }
        public double WhereFromSideSet { get; set; }
        public double WhereFromTopSet { get; set; }
        public double WhereFromWidth { get; set; }
        public double IndexFromSideSet { get; set; }
        public double IndexFromTopSet { get; set; }

        public double WhoSideSet { get; set; }
        public double WhoTopSet { get; set; }
        public double WhoWidth { get; set; }
        public double WhereSideSet { get; set; }
        public double WhereTopSet { get; set; }
        public double WhereWidth { get; set; }
        public double IndexSideSet { get; set; }
        public double IndexTopSet { get; set; }

        public double BigIndexSideSet { get; set; }
        public double BigIndexTopSet { get; set; }
    }
}
