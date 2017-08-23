using System;
using System.Data.Entity.Migrations;
using BET.Market.Jobs.DataAccess.EF.Defaults;

namespace BET.Market.Jobs.DataAccess.EF
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
        }
    }
}