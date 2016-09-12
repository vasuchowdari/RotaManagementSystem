namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedZktDateTimesToShiftProfiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "PayrollReferenceNumber", c => c.String());
            AddColumn("dbo.ShiftProfile", "ZktStartDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.ShiftProfile", "ZktEndDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.UnprocessedZkTimeData", "RmsUserId");
            DropColumn("dbo.UnprocessedZkTimeData", "RmsSiteId");
            DropColumn("dbo.UnprocessedZkTimeData", "Overtime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UnprocessedZkTimeData", "Overtime", c => c.Time(precision: 7));
            AddColumn("dbo.UnprocessedZkTimeData", "RmsSiteId", c => c.Long(nullable: false));
            AddColumn("dbo.UnprocessedZkTimeData", "RmsUserId", c => c.Long(nullable: false));
            DropColumn("dbo.ShiftProfile", "ZktEndDateTime");
            DropColumn("dbo.ShiftProfile", "ZktStartDateTime");
            DropColumn("dbo.User", "PayrollReferenceNumber");
        }
    }
}
