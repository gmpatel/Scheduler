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
                            new IndexEntity { Code = "ASX20", Name = "ASX 20", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX50", Name = "ASX 50", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX100", Name = "ASX 100", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX200", Name = "ASX 200", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "ASX300", Name = "ASX 300", DateTimeCreated = dateTime },
                            new IndexEntity { Code = "AllOrds", Name = "All Ordinaries", DateTimeCreated = dateTime }
                        }   
                    }
                );            
                
                context.SaveChanges();
            }
        }
    }
}