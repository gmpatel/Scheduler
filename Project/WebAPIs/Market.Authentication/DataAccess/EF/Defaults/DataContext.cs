using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Market.Authentication.Core.Entities;
using Market.Authentication.DataAccess.EF.Interfaces;

namespace Market.Authentication.DataAccess.EF.Defaults
{
    public class DataContext : DbContext, IDataContext
    {
        private static long counter;

        public DataContext() : base("AuthMarkets")
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

        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<FamilyTypeEntity> FamilyTypes { get; set; }
        public DbSet<IncomeRangeEntity> IncomeRanges { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<StateEntity> States { get; set; }
        public DbSet<TokenEntity> Tokens { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ClientEntity>().ToTable("Clients");
            modelBuilder.Entity<ClientEntity>().Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ClientEntity>().Property(c => c.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<ClientEntity>().Property(c => c.Description).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<ClientEntity>().Property(c => c.Url).HasColumnType("nvarchar").HasMaxLength(256).IsOptional();
            modelBuilder.Entity<ClientEntity>().Property(c => c.Enabled).IsRequired();
            modelBuilder.Entity<ClientEntity>().Property(c => c.DateTimeCreated).IsRequired();
            modelBuilder.Entity<ClientEntity>().HasKey(c => new { c.Id });

            modelBuilder.Entity<FamilyTypeEntity>().ToTable("FamilyTypes");
            modelBuilder.Entity<FamilyTypeEntity>().Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<FamilyTypeEntity>().Property(f => f.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<FamilyTypeEntity>().Property(f => f.Description).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<FamilyTypeEntity>().Property(k => k.Enabled).IsRequired();
            modelBuilder.Entity<FamilyTypeEntity>().Property(f => f.DateTimeCreated).IsRequired();
            modelBuilder.Entity<FamilyTypeEntity>().HasKey(r => new { r.Id });

            modelBuilder.Entity<IncomeRangeEntity>().ToTable("IncomeRanges");
            modelBuilder.Entity<IncomeRangeEntity>().Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<IncomeRangeEntity>().Property(f => f.Name).HasColumnType("nvarchar").HasMaxLength(256);
            modelBuilder.Entity<IncomeRangeEntity>().Property(f => f.Description).HasColumnType("nvarchar").HasMaxLength(1024);
            modelBuilder.Entity<IncomeRangeEntity>().Property(k => k.Enabled).IsRequired();
            modelBuilder.Entity<IncomeRangeEntity>().Property(f => f.DateTimeCreated).IsRequired();
            modelBuilder.Entity<IncomeRangeEntity>().HasKey(r => new { r.Id });

            modelBuilder.Entity<RoleEntity>().ToTable("Roles");
            modelBuilder.Entity<RoleEntity>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RoleEntity>().Property(r => r.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<RoleEntity>().Property(r => r.Description).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<RoleEntity>().Property(k => k.Enabled).IsRequired();
            modelBuilder.Entity<RoleEntity>().Property(r => r.DateTimeCreated).IsRequired();
            modelBuilder.Entity<RoleEntity>().HasKey(r => new { r.Id });

            modelBuilder.Entity<StateEntity>().ToTable("States");
            modelBuilder.Entity<StateEntity>().Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StateEntity>().Property(f => f.Name).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<StateEntity>().Property(f => f.Description).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<StateEntity>().Property(k => k.Enabled).IsRequired();
            modelBuilder.Entity<StateEntity>().Property(f => f.DateTimeCreated).IsRequired();
            modelBuilder.Entity<StateEntity>().HasKey(r => new { r.Id });

            modelBuilder.Entity<TokenEntity>().ToTable("Tokens");
            modelBuilder.Entity<TokenEntity>().Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<TokenEntity>().Property(t => t.Token).HasColumnType("nvarchar").IsRequired();
            modelBuilder.Entity<TokenEntity>().Property(t => t.DateTimeCreated).IsRequired();
            modelBuilder.Entity<TokenEntity>().Property(t => t.DateTimeExpire).IsRequired();
            modelBuilder.Entity<TokenEntity>().HasKey(r => new { r.Id });

            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<UserEntity>().Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UserEntity>().Property(u => u.FirstName).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.LastName).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.BirthDate).IsOptional();
            modelBuilder.Entity<UserEntity>().Property(u => u.FamilyTypeId).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.StateId).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.PostCode).HasColumnType("nvarchar").HasMaxLength(50).IsOptional();
            modelBuilder.Entity<UserEntity>().Property(u => u.Mobile).HasColumnType("nvarchar").HasMaxLength(50).IsOptional();
            modelBuilder.Entity<UserEntity>().Property(u => u.Email).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.Password).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.Key).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.Code).HasColumnType("nvarchar").HasMaxLength(256).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.RoleId).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.Enabled).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.IncomeRangeId).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.Verified).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.DateTimeCreated).IsRequired();
            modelBuilder.Entity<UserEntity>().HasKey(u => new { u.Id });

            modelBuilder.Entity<UserEntity>()
                .HasMany<ClientEntity>(u => u.Clients)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("ClientId");
                    cs.ToTable("UsersClients");
                });

            modelBuilder.Entity<FamilyTypeEntity>()
                .HasMany(f => f.Users)
                .WithRequired()
                .HasForeignKey(x => x.FamilyTypeId);

            modelBuilder.Entity<IncomeRangeEntity>()
                .HasMany(i => i.Users)
                .WithRequired()
                .HasForeignKey(x => x.IncomeRangeId);

            modelBuilder.Entity<RoleEntity>()
                .HasMany(r => r.Users)
                .WithRequired()
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<StateEntity>()
                .HasMany(f => f.Users)
                .WithRequired()
                .HasForeignKey(x => x.StateId);

            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Tokens)
                .WithRequired()
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<ClientEntity>().HasIndex(
                "IX_NC_UNIQ_Client_Name",
                IndexOptions.Unique,
                e => e.Property(x => x.Name)
            );

            modelBuilder.Entity<FamilyTypeEntity>().HasIndex(
                "IX_NC_UNIQ_FamilyType_Name",
                IndexOptions.Unique,
                e => e.Property(x => x.Name)
            );

            modelBuilder.Entity<IncomeRangeEntity>().HasIndex(
                "IX_NC_UNIQ_IncomeRange_Name",
                IndexOptions.Unique,
                e => e.Property(x => x.Name)
            );

            modelBuilder.Entity<RoleEntity>().HasIndex(
                "IX_NC_UNIQ_Role_Name",
                IndexOptions.Unique,
                e => e.Property(x => x.Name)
            );

            modelBuilder.Entity<StateEntity>().HasIndex(
                "IX_NC_UNIQ_State_Name",
                IndexOptions.Unique,
                e => e.Property(x => x.Name)
            );

            modelBuilder.Entity<TokenEntity>().HasIndex(
                "IX_NC_UNIQ_Token_UserId_Token",
                IndexOptions.Unique,
                e => e.Property(x => x.UserId),
                e => e.Property(x => x.Token)
            );

            modelBuilder.Entity<UserEntity>().HasIndex(
                "IX_NC_UNIQ_User_Email",
                IndexOptions.Unique,
                e => e.Property(x => x.Email)
            );
        }
    }
}