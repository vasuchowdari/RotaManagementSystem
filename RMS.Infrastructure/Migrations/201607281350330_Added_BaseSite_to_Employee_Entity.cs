namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_BaseSite_to_Employee_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgencyWorker", "BaseSiteId", c => c.Long(nullable: false));
            AddColumn("dbo.AgencyWorker", "BaseSubSiteId", c => c.Long());
            AddColumn("dbo.Employee", "BaseSiteId", c => c.Long(nullable: false));
            AddColumn("dbo.Employee", "BaseSubSiteId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "BaseSubSiteId");
            DropColumn("dbo.Employee", "BaseSiteId");
            DropColumn("dbo.AgencyWorker", "BaseSubSiteId");
            DropColumn("dbo.AgencyWorker", "BaseSiteId");
        }
    }
}
