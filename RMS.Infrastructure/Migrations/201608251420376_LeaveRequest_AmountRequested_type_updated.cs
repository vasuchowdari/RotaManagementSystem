namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeaveRequest_AmountRequested_type_updated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LeaveRequest", "AmountRequested", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeaveRequest", "AmountRequested", c => c.Double(nullable: false));
        }
    }
}
