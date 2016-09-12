namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_UserDefinedSystemConfig_Entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDefinedSystemConfig",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        NHoursAgo = c.Double(nullable: false),
                        ShiftPostStartStillValidThresholdValue = c.Double(nullable: false),
                        ShiftPreStartEarlyInThresholdValue = c.Double(nullable: false),
                        ShiftPostEndValidThresholdValue = c.Double(nullable: false),
                        NICFactor = c.Double(nullable: false),
                        AccruedHolidayFactor = c.Double(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Site", "PayrollEndDate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "PayrollEndDate");
            DropTable("dbo.UserDefinedSystemConfig");
        }
    }
}
