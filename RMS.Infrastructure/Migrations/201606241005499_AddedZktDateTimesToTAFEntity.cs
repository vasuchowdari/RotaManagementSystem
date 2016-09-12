namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedZktDateTimesToTAFEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeAdjustmentForm", "ZktStartDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.TimeAdjustmentForm", "ZktEndDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeAdjustmentForm", "ZktEndDateTime");
            DropColumn("dbo.TimeAdjustmentForm", "ZktStartDateTime");
        }
    }
}
