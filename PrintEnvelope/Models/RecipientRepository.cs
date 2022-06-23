using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public class RecipientRepository : IRecipientRepository
    {
        ApplicationDbContext dbContext;
        public string Filter { get; set; }

        public RecipientRepository()
        {
            dbContext = new ApplicationDbContext();
            dbContext.Recipients.Load();
        }

        public IEnumerable<Recipient> GetAll()
        {
            return dbContext.Recipients.ToList();
        }

        public void Update(Recipient recipient)
        {
            dbContext.Entry(recipient).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Remove(Recipient recipient)
        {
            dbContext.Recipients.Remove(recipient);
            dbContext.SaveChanges();
        }

        public void Add(Recipient recipient)
        {
            dbContext.Recipients.Add(recipient);
            dbContext.SaveChanges();
        }

        public void Clear()
        {
            dbContext.Database.ExecuteSqlCommand("delete from Recipients");
            dbContext.SaveChanges();
        }
    }
}
