using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.DataAccess.EF.Defaults;

namespace ASX.Market.Jobs.DataAccess.EF
{
    public class DBMigrationConfiguration : DbMigrationsConfiguration<DataContext>
    {
        private const string ConfigDbMigrationDirectory = @"DataAccess\EF\Migrations\DBContextMigrations";

        public DBMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

            MigrationsDirectory = ConfigDbMigrationDirectory;
        }

        protected override void Seed(DataContext context)
        {
            var dateTime = DateTime.UtcNow;

            if (!context.Exchanges.Any())
            {
                context.Exchanges.Add(new ExchangeEntity
                    {
                        Code = "ASX",
                        Name = "Australia Stock Exchange",
                        DateTimeCreated = dateTime,
                        Indices = new List<IndexEntity>
                        {
                            new IndexEntity { Code = "ASX20", Name = "ASX 20", Url = "http://www.marketindex.com.au/asx20", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX50", Name = "ASX 50", Url = "http://www.marketindex.com.au/asx50", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX100", Name = "ASX 100", Url = "http://www.marketindex.com.au/asx100", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX200", Name = "ASX 200", Url = "http://www.marketindex.com.au/asx200", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX300", Name = "ASX 300", Url = "http://www.marketindex.com.au/asx300", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "AllOrds", Name = "All Ordinaries", Url = "http://www.marketindex.com.au/all-ordinaries", DateTimeCreated = dateTime }
                        }   
                    }
                );            
                
                context.SaveChanges();
            }
        }
    }
}