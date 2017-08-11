using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;

namespace ASX.Market.Jobs.DataAccess.EF.Defaults
{
    public class DataContext : DbContext, IDataContext
    {
        private static long counter;

        public DataContext() : base("ASXMarkets")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;

            Database.SetInitializer<DataContext>
            (
                new MigrateDatabaseToLatestVersionExtended<DataContext, DBMigrationConfiguration>()
            );

            Id = ++counter;
        }

        public long Id { get; private set; }
        public long Instances => counter;
        public new virtual DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class => base.Entry(entity);

        public DbSet<ExchangeEntity> Exchanges { get; set; }
        public DbSet<IndexEntity> Indices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ExchangeEntity>().ToTable("Exchanges");
            modelBuilder.Entity<ExchangeEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ExchangeEntity>().Property(x => x.Code).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<ExchangeEntity>().Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<ExchangeEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<ExchangeEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<IndexEntity>().ToTable("Indices");
            modelBuilder.Entity<IndexEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<IndexEntity>().Property(x => x.Code).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<IndexEntity>().Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<IndexEntity>().Property(x => x.ExchangeId).IsRequired();
            modelBuilder.Entity<IndexEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<IndexEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<ExchangeEntity>()
                .HasMany(x => x.Indices)
                .WithRequired()
                .HasForeignKey(x => x.ExchangeId);
        }
    }
}