namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Start_End_Dates_to_Contract_and_Employee_Entities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contract", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contract", "EndDate", c => c.DateTime());
            AddColumn("dbo.Employee", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employee", "LeaveDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "LeaveDate");
            DropColumn("dbo.Employee", "StartDate");
            DropColumn("dbo.Contract", "EndDate");
            DropColumn("dbo.Contract", "StartDate");
        }
    }
}
