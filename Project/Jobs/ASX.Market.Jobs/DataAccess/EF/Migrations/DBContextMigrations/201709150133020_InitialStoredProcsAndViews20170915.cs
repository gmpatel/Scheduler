namespace ASX.Market.Jobs.DataAccess.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialStoredProcsAndViews20170915 : DbMigration
    {
        public override void Up()
        {
            Sql(Properties.Resources.InitialStoredProcsAndViews20170915_Up);
        }
        
        public override void Down()
        {
            Sql(Properties.Resources.InitialStoredProcsAndViews20170915_Down);
        }
    }
}