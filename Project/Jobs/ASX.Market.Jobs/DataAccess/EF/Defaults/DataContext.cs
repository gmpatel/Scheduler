using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.Core.Entities.Views;
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
        //public DbSet<IndexDetailEntity> IndexDetails { get; set; }
        public DbSet<StockEntity> Stocks { get; set; }
        public DbSet<StockDetailEntity> StockDetails { get; set; }
        public DbSet<StockDetailAggregatedEntity> StockDetailsAggregated { get; set; }
        public DbSet<StockDetailAggregatedLatestMovementEntity> StockDetailAggregatedLatestMovements { get; set; }
        
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
            modelBuilder.Entity<StockEntity>().Property(x => x.Flag1).IsOptional();
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

            modelBuilder.Entity<StockDetailAggregatedEntity>().ToTable("StockDetailsAggregated");
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StockId).IsRequired();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MovementDays).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MovementDirection).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartDate).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartDay).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartIndicator).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndDate).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndDay).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndIndicator).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.Changed).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.ChangedPercent).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallChanged).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallChangedPercent).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MaxPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MinPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallMaxPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallMinPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<StockDetailAggregatedLatestMovementEntity>().ToTable("StockDetailsAggregatedLatestMovement");
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StockId).IsRequired();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MovementDays).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MovementDirection).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartDate).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartDay).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.StartIndicator).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndDate).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndDay).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.EndIndicator).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.Changed).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.ChangedPercent).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallChanged).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallChangedPercent).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MaxPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.MinPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallMaxPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().Property(x => x.OverallMinPrice).IsOptional();
            modelBuilder.Entity<StockDetailAggregatedEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<ExchangeEntity>()
                .HasMany(x => x.Indices)
                .WithRequired()
                .HasForeignKey(x => x.ExchangeId);

            modelBuilder.Entity<StockEntity>()
                .HasMany(x => x.StockDetails)
                .WithRequired()
                .HasForeignKey(x => x.StockId);

            modelBuilder.Entity<StockEntity>()
                .HasMany(x => x.StockDetailAggregated)
                .WithRequired()
                .HasForeignKey(x => x.StockId);

            modelBuilder.Entity<StockEntity>()
                .HasMany(x => x.StockDetailAggregatedLatestMovements)
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

            modelBuilder.Entity<StockDetailAggregatedEntity>().HasIndex(
                "IX_NC_UNIQ_StockDetailAggregated_Date_StockId",
                IndexOptions.Unique,
                k => k.Property(x => x.StartDate),
                k => k.Property(x => x.StockId)
            );

            modelBuilder.Entity<StockDetailAggregatedLatestMovementEntity>().HasIndex(
                "IX_NC_UNIQ_StockDetailAggregatedLatestMovement_Date_StockId",
                IndexOptions.Unique,
                k => k.Property(x => x.StartDate),
                k => k.Property(x => x.StockId)
            );
        }
    }
}