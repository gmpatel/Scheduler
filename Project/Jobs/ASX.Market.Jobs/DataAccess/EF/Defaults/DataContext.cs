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

            modelBuilder.Entity<StockEntity>().ToTable("Stocks");
            modelBuilder.Entity<StockEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StockEntity>().Property(x => x.Code).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<StockEntity>().Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<StockEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<StockEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<StockDetailEntity>().ToTable("StockDetails");
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Date).IsRequired();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.StockId).IsRequired();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Price).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Change).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.ChangePercent).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.High).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Low).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Volume).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.MarketCapital).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.OneYearChange).IsOptional();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.Flag1).IsRequired();
            modelBuilder.Entity<StockDetailEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<StockDetailEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<ExchangeEntity>()
                .HasMany(x => x.Indices)
                .WithRequired()
                .HasForeignKey(x => x.ExchangeId);

            modelBuilder.Entity<StockEntity>()
                .HasMany(x => x.StockDetails)
                .WithRequired()
                .HasForeignKey(x => x.StockId);

            modelBuilder.Entity<IndexEntity>()
                .HasMany<StockEntity>(u => u.Stocks)
                .WithMany(c => c.Indices)
                .Map(cs =>
                {
                    cs.MapLeftKey("IndexId");
                    cs.MapRightKey("StockId");
                    cs.ToTable("IndicesStocks");
                });

            modelBuilder.Entity<ExchangeEntity>().HasIndex(
                "IX_NC_UNIQ_Exchange_Code",
                IndexOptions.Unique,
                k => k.Property(x => x.Code)
            );

            modelBuilder.Entity<IndexEntity>().HasIndex(
                "IX_NC_UNIQ_Index_Code",
                IndexOptions.Unique,
                k => k.Property(x => x.Code)
            );

            modelBuilder.Entity<StockEntity>().HasIndex(
                "IX_NC_UNIQ_Stock_Code",
                IndexOptions.Unique,
                k => k.Property(x => x.Code)
            );

            modelBuilder.Entity<StockDetailEntity>().HasIndex(
                "IX_NC_UNIQ_StockDetail_Date_StockId",
                IndexOptions.Unique,
                k => k.Property(x => x.Date),
                k => k.Property(x => x.StockId)
            );
        }
    }
}