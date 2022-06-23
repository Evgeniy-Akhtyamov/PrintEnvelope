using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public interface IRecipientRepository
    {
        IEnumerable<Recipient> GetAll();
        void Add(Recipient recipient);
        void Update(Recipient recipient);
        void Remove(Recipient recipient);
        void Clear();
    }
}
