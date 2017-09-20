using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Market.Authentication.Core.Entities;
using Market.Authentication.DataAccess.EF.Defaults;

namespace Market.Authentication.DataAccess.EF
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

            if (!context.FamilyTypes.Any())
            {
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('FamilyTypes', RESEED, 0)");

                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Prefer not to disclose", Description = "User has preferred not to disclose this information", Enabled = false, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Single", Description = "Single person", Enabled = true, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Single parent", Description = "Single parent having children but no spouse", Enabled = true, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Single parent plus", Description = "Single parent having children but and spouse with other family adult member", Enabled = true, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Couple", Description = "Couple", Enabled = true, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Family", Description = "Couple having children", Enabled = true, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Family plus", Description = "Couple having children with other family adult member", Enabled = true, DateTimeCreated = dateTime });
                context.FamilyTypes.Add(new FamilyTypeEntity { Name = "Dependants only", Description = "Single parents who only care for childrens", Enabled = true, DateTimeCreated = dateTime });

                context.SaveChanges();
            }

            if (!context.IncomeRanges.Any())
            {
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('IncomeRanges', RESEED, 0)");

                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "Prefer not to disclose", Description = "User has preferred not to disclose this information", Enabled = false, DateTimeCreated = dateTime });
                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "$ 18,201 – $ 37,000", Description = "Low income", Enabled = true, DateTimeCreated = dateTime });
                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "$ 37,001 – $ 80,000", Description = "Moderate income", Enabled = true, DateTimeCreated = dateTime });
                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "$ 80,001 – $180,000", Description = "Good income", Enabled = true, DateTimeCreated = dateTime });
                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "$180,001 – $350,000", Description = "High income", Enabled = true, DateTimeCreated = dateTime });
                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "$350,001 – $500,000", Description = "Ultra high income", Enabled = true, DateTimeCreated = dateTime });
                context.IncomeRanges.Add(new IncomeRangeEntity { Name = "$500,000 +", Description = "Executive income", Enabled = true, DateTimeCreated = dateTime });

                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Roles', RESEED, 0)");

                context.Roles.Add(new RoleEntity { Name = "None", Description = "None of the roles has not been allocated to the user", Enabled = false, DateTimeCreated = dateTime });
                context.Roles.Add(new RoleEntity { Name = "User", Description = "The user has granted the rights as a normal user", Enabled = true, DateTimeCreated = dateTime });
                context.Roles.Add(new RoleEntity { Name = "Admin", Description = "The user has granted the rights as an admin", Enabled = true, DateTimeCreated = dateTime });
                
                context.SaveChanges();
            }

            if (!context.States.Any())
            {
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('States', RESEED, 0)");

                context.States.Add(new StateEntity { Name = "Prefer not to disclose", Description = "NON", Enabled = false, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Australian Capital Territory", Description = "ACT", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "New South Wales", Description = "NSW", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Victoria", Description = "VIC", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Queensland", Description = "QLD", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Western Australia", Description = "WA", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "South Australia", Description = "SA", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Northern Territory", Description = "NT", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Tasmania", Description = "TAS", Enabled = true, DateTimeCreated = dateTime });
                context.States.Add(new StateEntity { Name = "Other (Non Australian)", Description = "OTH", Enabled = false, DateTimeCreated = dateTime });

                context.SaveChanges();
            }
        }
    }
}