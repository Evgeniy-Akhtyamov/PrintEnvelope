using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public interface IPrintSettingsRepository
    {
        IEnumerable<PrintSettings> GetAll();
        void Update(PrintSettings printSettings);
        void Remove(PrintSettings printSettings);
        void Add(PrintSettings printSettings);
    }
}
