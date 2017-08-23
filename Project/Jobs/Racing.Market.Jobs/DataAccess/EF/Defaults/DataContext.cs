using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using BET.Market.Jobs.Core.Entities;
using BET.Market.Jobs.DataAccess.EF.Interfaces;

namespace BET.Market.Jobs.DataAccess.EF.Defaults
{
    public class DataContext : DbContext, IDataContext
    {
        private static long counter;

        public DataContext() : base("BETMarkets")
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

        public DbSet<MeetingEntity> Meetings { get; set; }
        public DbSet<RaceEntity> Races { get; set; }
        public DbSet<RunnerEntity> Runners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<VenueEntity>().ToTable("Venues");
            modelBuilder.Entity<VenueEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<VenueEntity>().Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<VenueEntity>().Property(x => x.Name2).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<VenueEntity>().Property(x => x.Name3).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<VenueEntity>().Property(x => x.Province).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<VenueEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<VenueEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<MeetingEntity>().ToTable("Meetings");
            modelBuilder.Entity<MeetingEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MeetingEntity>().Property(x => x.Date).IsRequired();
            modelBuilder.Entity<MeetingEntity>().Property(x => x.VenueId).IsRequired();
            modelBuilder.Entity<MeetingEntity>().Property(x => x.SkyId).IsOptional();
            modelBuilder.Entity<MeetingEntity>().Property(x => x.FormUrl).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<MeetingEntity>().Property(x => x.TipsUrl).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<MeetingEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<MeetingEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<RaceEntity>().ToTable("Races");
            modelBuilder.Entity<RaceEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RaceEntity>().Property(x => x.MeetingId).IsRequired();
            modelBuilder.Entity<RaceEntity>().Property(x => x.Number).IsRequired();
            modelBuilder.Entity<RaceEntity>().Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<RaceEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<RaceEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<RunnerEntity>().ToTable("Runners");
            modelBuilder.Entity<RunnerEntity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RunnerEntity>().Property(x => x.RaceId).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.Number).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.IsGood).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormSkyRating).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormBest12Months).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormRecent).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormDistance).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormClass).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormTimeRating).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormInWet).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.FormBestOverall).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.TipSky).IsRequired();
            modelBuilder.Entity<RunnerEntity>().Property(x => x.DateTimeCreated).IsRequired();
            modelBuilder.Entity<RunnerEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<VenueEntity>()
                .HasMany(x => x.Meetings)
                .WithRequired()
                .HasForeignKey(x => x.VenueId);

            modelBuilder.Entity<MeetingEntity>()
                .HasMany(x => x.Races)
                .WithRequired()
                .HasForeignKey(x => x.MeetingId);

            modelBuilder.Entity<RaceEntity>()
                .HasMany(x => x.Runners)
                .WithRequired()
                .HasForeignKey(x => x.RaceId);

            modelBuilder.Entity<VenueEntity>().HasIndex(
                "IX_NC_UNIQ_Venue_Name_Province",
                IndexOptions.Unique,
                k => k.Property(x => x.Name),
                k => k.Property(x => x.Province)
            );

            modelBuilder.Entity<MeetingEntity>().HasIndex(
                "IX_NC_UNIQ_Meeting_Date_VenueId",
                IndexOptions.Unique,
                k => k.Property(x => x.Date),
                k => k.Property(x => x.VenueId)
            );

            modelBuilder.Entity<RaceEntity>().HasIndex(
                "IX_NC_UNIQ_Race_Number_MeetingId",
                IndexOptions.Unique,
                k => k.Property(x => x.Name),
                k => k.Property(x => x.MeetingId)
            );

            modelBuilder.Entity<RunnerEntity>().HasIndex(
                "IX_NC_UNIQ_Runner_Number_RaceId",
                IndexOptions.Unique,
                k => k.Property(x => x.Number),
                k => k.Property(x => x.RaceId)
            );
        }
    }
}