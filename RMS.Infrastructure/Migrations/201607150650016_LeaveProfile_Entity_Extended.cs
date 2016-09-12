namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeaveProfile_Entity_Extended : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonnelLeaveProfile", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PersonnelLeaveProfile", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PersonnelLeaveProfile", "NumberOfHoursTaken", c => c.Double(nullable: false));
            AddColumn("dbo.PersonnelLeaveProfile", "NumberOfHoursAllocated", c => c.Double(nullable: false));
            AddColumn("dbo.PersonnelLeaveProfile", "NumberOfHoursRemaining", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonnelLeaveProfile", "NumberOfHoursRemaining");
            DropColumn("dbo.PersonnelLeaveProfile", "NumberOfHoursAllocated");
            DropColumn("dbo.PersonnelLeaveProfile", "NumberOfHoursTaken");
            DropColumn("dbo.PersonnelLeaveProfile", "EndDate");
            DropColumn("dbo.PersonnelLeaveProfile", "StartDate");
        }
    }
}
