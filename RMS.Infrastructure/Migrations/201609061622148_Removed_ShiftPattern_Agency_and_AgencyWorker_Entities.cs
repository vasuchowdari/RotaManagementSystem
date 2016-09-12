namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_ShiftPattern_Agency_and_AgencyWorker_Entities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgencyWorker", "AgencyId", "dbo.Agency");
            DropForeignKey("dbo.Contract", "AgencyWorkerId", "dbo.AgencyWorker");
            DropForeignKey("dbo.PersonnelLeaveProfile", "AgencyWorkerId", "dbo.AgencyWorker");
            DropForeignKey("dbo.LeaveRequest", "AgencyWorkerId", "dbo.AgencyWorker");
            DropForeignKey("dbo.AgencyWorker", "UserId", "dbo.User");
            DropForeignKey("dbo.ShiftPattern", "CalendarResourceRequirementId", "dbo.CalendarResourceRequirement");
            DropForeignKey("dbo.Shift", "ShiftPatternId", "dbo.ShiftPattern");
            DropIndex("dbo.AgencyWorker", new[] { "AgencyId" });
            DropIndex("dbo.AgencyWorker", new[] { "UserId" });
            DropIndex("dbo.Contract", new[] { "AgencyWorkerId" });
            DropIndex("dbo.PersonnelLeaveProfile", new[] { "AgencyWorkerId" });
            DropIndex("dbo.LeaveRequest", new[] { "AgencyWorkerId" });
            DropIndex("dbo.ShiftPattern", new[] { "CalendarResourceRequirementId" });
            DropIndex("dbo.Shift", new[] { "ShiftPatternId" });
            AddColumn("dbo.Shift", "CalendarResourceRequirementId", c => c.Long(nullable: false));
            CreateIndex("dbo.Shift", "CalendarResourceRequirementId");
            AddForeignKey("dbo.Shift", "CalendarResourceRequirementId", "dbo.CalendarResourceRequirement", "Id", cascadeDelete: true);
            DropColumn("dbo.Contract", "AgencyWorkerId");
            DropColumn("dbo.PersonnelLeaveProfile", "AgencyWorkerId");
            DropColumn("dbo.LeaveRequest", "AgencyWorkerId");
            DropColumn("dbo.Shift", "ShiftPatternId");
            DropColumn("dbo.Shift", "AgencyWorkerId");
            DropColumn("dbo.ShiftProfile", "AgencyWorkerId");
            DropColumn("dbo.SitePersonnelLookup", "AgencyWorkerId");
            DropColumn("dbo.TimeAdjustmentForm", "AgencyWorkerId");
            DropTable("dbo.Agency");
            DropTable("dbo.AgencyWorker");
            DropTable("dbo.ShiftPattern");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShiftPattern",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CalendarResourceRequirementId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AgencyWorker",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AgencyId = c.Long(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        Postcode = c.String(),
                        Telephone = c.String(),
                        Mobile = c.String(),
                        GenderId = c.Long(nullable: false),
                        BaseSiteId = c.Long(nullable: false),
                        BaseSubSiteId = c.Long(),
                        UserId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Agency",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsApproved = c.Boolean(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Postcode = c.String(),
                        Telephone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TimeAdjustmentForm", "AgencyWorkerId", c => c.Long());
            AddColumn("dbo.SitePersonnelLookup", "AgencyWorkerId", c => c.Long());
            AddColumn("dbo.ShiftProfile", "AgencyWorkerId", c => c.Long());
            AddColumn("dbo.Shift", "AgencyWorkerId", c => c.Long());
            AddColumn("dbo.Shift", "ShiftPatternId", c => c.Long(nullable: false));
            AddColumn("dbo.LeaveRequest", "AgencyWorkerId", c => c.Long());
            AddColumn("dbo.PersonnelLeaveProfile", "AgencyWorkerId", c => c.Long());
            AddColumn("dbo.Contract", "AgencyWorkerId", c => c.Long());
            DropForeignKey("dbo.Shift", "CalendarResourceRequirementId", "dbo.CalendarResourceRequirement");
            DropIndex("dbo.Shift", new[] { "CalendarResourceRequirementId" });
            DropColumn("dbo.Shift", "CalendarResourceRequirementId");
            CreateIndex("dbo.Shift", "ShiftPatternId");
            CreateIndex("dbo.ShiftPattern", "CalendarResourceRequirementId");
            CreateIndex("dbo.LeaveRequest", "AgencyWorkerId");
            CreateIndex("dbo.PersonnelLeaveProfile", "AgencyWorkerId");
            CreateIndex("dbo.Contract", "AgencyWorkerId");
            CreateIndex("dbo.AgencyWorker", "UserId");
            CreateIndex("dbo.AgencyWorker", "AgencyId");
            AddForeignKey("dbo.Shift", "ShiftPatternId", "dbo.ShiftPattern", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ShiftPattern", "CalendarResourceRequirementId", "dbo.CalendarResourceRequirement", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AgencyWorker", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LeaveRequest", "AgencyWorkerId", "dbo.AgencyWorker", "Id");
            AddForeignKey("dbo.PersonnelLeaveProfile", "AgencyWorkerId", "dbo.AgencyWorker", "Id");
            AddForeignKey("dbo.Contract", "AgencyWorkerId", "dbo.AgencyWorker", "Id");
            AddForeignKey("dbo.AgencyWorker", "AgencyId", "dbo.Agency", "Id", cascadeDelete: true);
        }
    }
}
