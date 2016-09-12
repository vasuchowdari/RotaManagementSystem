namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBoolFieldsToTimeFormsAndShiftProfiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShiftProfile", "IsModified", c => c.Boolean(nullable: false));
            AddColumn("dbo.TimeAdjustmentForm", "IsManagerApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.TimeAdjustmentForm", "IsAdminApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeAdjustmentForm", "IsAdminApproved");
            DropColumn("dbo.TimeAdjustmentForm", "IsManagerApproved");
            DropColumn("dbo.ShiftProfile", "IsModified");
        }
    }
}
