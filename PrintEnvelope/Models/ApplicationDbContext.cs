using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {}

        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<PrintSettings> PrintSettings { get; set; }        
    }
}
