namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Status_to_LeaveRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveRequest", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveRequest", "Status");
        }
    }
}
