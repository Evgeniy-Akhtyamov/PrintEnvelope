using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.Models
{
    public class PrintSettingsRepository : IPrintSettingsRepository
    {
        ApplicationDbContext dbContext;

        public PrintSettingsRepository()
        {
            dbContext = new ApplicationDbContext();            
            dbContext.PrintSettings.Load();
            if (dbContext.PrintSettings.ToList().Count == 0)
                AddDefaultSettings();
        }        

        public IEnumerable<PrintSettings> GetAll()
        {
            return dbContext.PrintSettings.ToList();
        }

        public void Add(PrintSettings printSettings)
        {
            dbContext.PrintSettings.Add(printSettings);
            dbContext.SaveChanges();
        }

        public void Remove(PrintSettings printSettings)
        {
            dbContext.PrintSettings.Remove(printSettings);
            dbContext.SaveChanges();
        }

        public void Update(PrintSettings printSettings)
        {
            dbContext.Entry(printSettings).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        private void AddDefaultSettings()
        {
            dbContext.PrintSettings.Add(
                new PrintSettings
                {
                    EnvelopeFormat = "DL",
                    EnvelopeWidth = 220,
                    EnvelopeHeight = 110,
                    WhoFromSideSet = 21,
                    WhoFromTopSet = 6,
                    WhoFromWidth = 120,
                    WhereFromSideSet = 21,
                    WhereFromTopSet = 12,
                    WhereFromWidth = 70,
                    IndexFromSideSet = 60,
                    IndexFromTopSet = 26,
                    WhoSideSet = 115,
                    WhoTopSet = 47,
                    WhoWidth = 85,
                    WhereSideSet = 115,
                    WhereTopSet = 60,
                    WhereWidth = 85,
                    IndexSideSet = 110,
                    IndexTopSet = 87,
                    BigIndexSideSet = 18,
                    BigIndexTopSet = 85,
                });
            dbContext.PrintSettings.Add(
                new PrintSettings
                {
                    EnvelopeFormat = "C6",
                    EnvelopeWidth = 162,
                    EnvelopeHeight = 114,
                    WhoFromSideSet = 18,
                    WhoFromTopSet = 5,
                    WhoFromWidth = 120,
                    WhereFromSideSet = 18,
                    WhereFromTopSet = 11,
                    WhereFromWidth = 70,
                    IndexFromSideSet = 43,
                    IndexFromTopSet = 30,
                    WhoSideSet = 84,
                    WhoTopSet = 50,
                    WhoWidth = 70,
                    WhereSideSet = 84,
                    WhereTopSet = 63,
                    WhereWidth = 70,
                    IndexSideSet = 78,
                    IndexTopSet = 90,
                    BigIndexSideSet = 17,
                    BigIndexTopSet = 87,
                });
            dbContext.SaveChanges();
        }
    }
}
