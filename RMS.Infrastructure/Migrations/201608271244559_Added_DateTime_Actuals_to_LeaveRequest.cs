namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_DateTime_Actuals_to_LeaveRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveRequest", "ActualStartDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LeaveRequest", "ActualEndDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LeaveRequest", "ZktStartDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LeaveRequest", "ZktEndDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveRequest", "ZktEndDateTime");
            DropColumn("dbo.LeaveRequest", "ZktStartDateTime");
            DropColumn("dbo.LeaveRequest", "ActualEndDateTime");
            DropColumn("dbo.LeaveRequest", "ActualStartDateTime");
        }
    }
}
