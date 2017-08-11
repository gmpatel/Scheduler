using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace ASX.Market.Jobs.DataAccess.EF
{
    public class MigrateDatabaseToLatestVersionExtended<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext> where TContext : DbContext where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private readonly DbMigrationsConfiguration configuration;

        public MigrateDatabaseToLatestVersionExtended()
        {
            configuration = new TMigrationsConfiguration();
        }

        public MigrateDatabaseToLatestVersionExtended(string connectionString) // Set the TargetDatabase for migrations to use the supplied connection string
        {
            configuration = new TMigrationsConfiguration
            {
                TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient")
            };
        }

        public void InitializeDatabase(TContext context) // Update the migrator with the config containing the right connection string
        {
            var dbMigrator = new DbMigrator(configuration);
            dbMigrator.Update();
        }
    }
}