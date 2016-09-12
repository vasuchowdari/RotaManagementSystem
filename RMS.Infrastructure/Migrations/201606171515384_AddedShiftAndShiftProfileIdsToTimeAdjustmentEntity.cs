namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedShiftAndShiftProfileIdsToTimeAdjustmentEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeAdjustmentForm", "ShiftId", c => c.Long(nullable: false));
            AddColumn("dbo.TimeAdjustmentForm", "ShiftProfileId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeAdjustmentForm", "ShiftProfileId");
            DropColumn("dbo.TimeAdjustmentForm", "ShiftId");
        }
    }
}
