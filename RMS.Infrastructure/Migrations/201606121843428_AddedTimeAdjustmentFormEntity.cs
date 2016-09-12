namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimeAdjustmentFormEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeAdjustmentForm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        StaffName = c.String(),
                        ShiftLocation = c.String(),
                        ShiftStartDateTime = c.DateTime(nullable: false),
                        ShiftEndDateTime = c.DateTime(nullable: false),
                        ActualStartDateTime = c.DateTime(nullable: false),
                        ActualEndDateTime = c.DateTime(nullable: false),
                        MissedClockIn = c.Boolean(nullable: false),
                        MissedClockOut = c.Boolean(nullable: false),
                        LateIn = c.Boolean(nullable: false),
                        EarlyOut = c.Boolean(nullable: false),
                        Notes = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TimeAdjustmentForm");
        }
    }
}
