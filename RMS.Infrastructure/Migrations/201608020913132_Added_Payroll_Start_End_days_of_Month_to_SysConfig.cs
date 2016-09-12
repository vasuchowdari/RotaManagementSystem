namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Payroll_Start_End_days_of_Month_to_SysConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDefinedSystemConfig", "PayrollStartDayOfMonth", c => c.Int(nullable: false));
            AddColumn("dbo.UserDefinedSystemConfig", "PayrollEndDayOfMonth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDefinedSystemConfig", "PayrollEndDayOfMonth");
            DropColumn("dbo.UserDefinedSystemConfig", "PayrollStartDayOfMonth");
        }
    }
}
