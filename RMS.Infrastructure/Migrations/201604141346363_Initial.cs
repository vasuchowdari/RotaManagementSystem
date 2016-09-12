namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
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
                        UserId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agency", t => t.AgencyId, cascadeDelete: false)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.AgencyId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ResourceId = c.Long(nullable: false),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        WeeklyHours = c.Double(nullable: false),
                        BaseRateOverride = c.Double(),
                        OvertimeModifierOverride = c.Double(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgencyWorker", t => t.AgencyWorkerId)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId)
                .Index(t => t.AgencyWorkerId);
            
            CreateTable(
                "dbo.PersonnelLeaveProfile",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NumberOfDaysTaken = c.Double(nullable: false),
                        NumberOfDaysAllocated = c.Double(nullable: false),
                        NumberOfDaysRemaining = c.Double(nullable: false),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        LeaveTypeId = c.Long(nullable: false),
                        Notes = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgencyWorker", t => t.AgencyWorkerId)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId)
                .Index(t => t.AgencyWorkerId);
            
            CreateTable(
                "dbo.LeaveRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        AmountRequested = c.Double(nullable: false),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        LeaveTypeId = c.Long(nullable: false),
                        Notes = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        IsTaken = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgencyWorker", t => t.AgencyWorkerId)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId)
                .Index(t => t.AgencyWorkerId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        SystemAccessRoleId = c.Long(nullable: false),
                        IsAccountLocked = c.Boolean(nullable: false),
                        ExternalTimeSystemId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemAccessRole", t => t.SystemAccessRoleId, cascadeDelete: false)
                .Index(t => t.SystemAccessRoleId);
            
            CreateTable(
                "dbo.SystemAccessRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Long(nullable: false),
                        EntryDateTime = c.DateTime(nullable: false),
                        ActionType = c.String(),
                        TableName = c.String(),
                        RecordId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BudgetPeriod",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForecastTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualSpendTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BudgetId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budget", t => t.BudgetId, cascadeDelete: false)
                .Index(t => t.BudgetId);
            
            CreateTable(
                "dbo.Budget",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ForecastTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SiteId = c.Long(nullable: false),
                        SubSiteId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CalendarResourceRequirement",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CalendarId = c.Long(nullable: false),
                        ResourceId = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calendar", t => t.CalendarId, cascadeDelete: false)
                .ForeignKey("dbo.Resource", t => t.ResourceId, cascadeDelete: false)
                .Index(t => t.CalendarId)
                .Index(t => t.ResourceId);
            
            CreateTable(
                "dbo.Calendar",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        SiteId = c.Long(nullable: false),
                        SubSiteId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: false)
                .ForeignKey("dbo.SubSite", t => t.SubSiteId)
                .Index(t => t.SiteId)
                .Index(t => t.SubSiteId);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        BaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShiftPattern",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CalendarResourceRequirementId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CalendarResourceRequirement", t => t.CalendarResourceRequirementId, cascadeDelete: false)
                .Index(t => t.CalendarResourceRequirementId);
            
            CreateTable(
                "dbo.Shift",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsAssigned = c.Boolean(nullable: false),
                        ShiftTemplateId = c.Long(nullable: false),
                        ShiftPatternId = c.Long(nullable: false),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShiftPattern", t => t.ShiftPatternId, cascadeDelete: false)
                .ForeignKey("dbo.ShiftTemplate", t => t.ShiftTemplateId, cascadeDelete: false)
                .Index(t => t.ShiftTemplateId)
                .Index(t => t.ShiftPatternId);
            
            CreateTable(
                "dbo.ShiftTemplate",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ShiftRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShiftTypeId = c.Long(nullable: false),
                        ResourceId = c.Long(nullable: false),
                        SiteId = c.Long(nullable: false),
                        SubSiteId = c.Long(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Duration = c.Double(nullable: false),
                        UnpaidBreakDuration = c.Double(nullable: false),
                        Mon = c.Boolean(nullable: false),
                        Tue = c.Boolean(nullable: false),
                        Wed = c.Boolean(nullable: false),
                        Thu = c.Boolean(nullable: false),
                        Fri = c.Boolean(nullable: false),
                        Sat = c.Boolean(nullable: false),
                        Sun = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resource", t => t.ResourceId, cascadeDelete: false)
                .ForeignKey("dbo.ShiftType", t => t.ShiftTypeId, cascadeDelete: false)
                .Index(t => t.ShiftTypeId)
                .Index(t => t.ResourceId);
            
            CreateTable(
                "dbo.ShiftType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsOvernight = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContactName = c.String(),
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
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        EmployeeTypeId = c.Long(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        Postcode = c.String(),
                        Telephone = c.String(),
                        Mobile = c.String(),
                        GenderId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.EmployeeType", t => t.EmployeeTypeId, cascadeDelete: false)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.CompanyId)
                .Index(t => t.EmployeeTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmployeeType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeaveType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourceRateModifier",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Double(nullable: false),
                        ShiftTypeId = c.Long(nullable: false),
                        ResourceId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShiftProfile",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        ActualStartDateTime = c.DateTime(nullable: false),
                        ActualEndDateTime = c.DateTime(nullable: false),
                        HoursWorked = c.Long(nullable: false),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        ShiftId = c.Long(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shift", t => t.ShiftId, cascadeDelete: false)
                .Index(t => t.ShiftId);
            
            CreateTable(
                "dbo.SitePersonnelLookup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmployeeId = c.Long(),
                        AgencyWorkerId = c.Long(),
                        SiteId = c.Long(nullable: false),
                        SubSiteId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        PayrollStartDate = c.Int(nullable: false),
                        SiteTypeId = c.Long(nullable: false),
                        CompanyId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.SiteType", t => t.SiteTypeId, cascadeDelete: false)
                .Index(t => t.SiteTypeId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.SiteType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubSite",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        SubSiteTypeId = c.Long(nullable: false),
                        SiteId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: false)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.SubSiteType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UnprocessedZkTimeData",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClockingTime = c.DateTime(nullable: false),
                        ZkTimeUserId = c.Int(nullable: false),
                        ZkTimeBadgeNumber = c.Int(nullable: false),
                        ZkTimeUserName = c.String(),
                        ZkTimeSiteNumber = c.Int(nullable: false),
                        ZkTimeSiteName = c.String(),
                        RmsUserId = c.Long(nullable: false),
                        RmsSiteId = c.Long(nullable: false),
                        ShiftId = c.Long(),
                        Overtime = c.Time(precision: 7),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubSite", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Calendar", "SubSiteId", "dbo.SubSite");
            DropForeignKey("dbo.Site", "SiteTypeId", "dbo.SiteType");
            DropForeignKey("dbo.Site", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Calendar", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ShiftProfile", "ShiftId", "dbo.Shift");
            DropForeignKey("dbo.Employee", "UserId", "dbo.User");
            DropForeignKey("dbo.LeaveRequest", "EmployeeId", "dbo.Employee");
            DropForeignKey("dbo.PersonnelLeaveProfile", "EmployeeId", "dbo.Employee");
            DropForeignKey("dbo.Employee", "EmployeeTypeId", "dbo.EmployeeType");
            DropForeignKey("dbo.Contract", "EmployeeId", "dbo.Employee");
            DropForeignKey("dbo.Employee", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Shift", "ShiftTemplateId", "dbo.ShiftTemplate");
            DropForeignKey("dbo.ShiftTemplate", "ShiftTypeId", "dbo.ShiftType");
            DropForeignKey("dbo.ShiftTemplate", "ResourceId", "dbo.Resource");
            DropForeignKey("dbo.Shift", "ShiftPatternId", "dbo.ShiftPattern");
            DropForeignKey("dbo.ShiftPattern", "CalendarResourceRequirementId", "dbo.CalendarResourceRequirement");
            DropForeignKey("dbo.CalendarResourceRequirement", "ResourceId", "dbo.Resource");
            DropForeignKey("dbo.CalendarResourceRequirement", "CalendarId", "dbo.Calendar");
            DropForeignKey("dbo.BudgetPeriod", "BudgetId", "dbo.Budget");
            DropForeignKey("dbo.AgencyWorker", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "SystemAccessRoleId", "dbo.SystemAccessRole");
            DropForeignKey("dbo.LeaveRequest", "AgencyWorkerId", "dbo.AgencyWorker");
            DropForeignKey("dbo.PersonnelLeaveProfile", "AgencyWorkerId", "dbo.AgencyWorker");
            DropForeignKey("dbo.Contract", "AgencyWorkerId", "dbo.AgencyWorker");
            DropForeignKey("dbo.AgencyWorker", "AgencyId", "dbo.Agency");
            DropIndex("dbo.SubSite", new[] { "SiteId" });
            DropIndex("dbo.Site", new[] { "CompanyId" });
            DropIndex("dbo.Site", new[] { "SiteTypeId" });
            DropIndex("dbo.ShiftProfile", new[] { "ShiftId" });
            DropIndex("dbo.Employee", new[] { "UserId" });
            DropIndex("dbo.Employee", new[] { "EmployeeTypeId" });
            DropIndex("dbo.Employee", new[] { "CompanyId" });
            DropIndex("dbo.ShiftTemplate", new[] { "ResourceId" });
            DropIndex("dbo.ShiftTemplate", new[] { "ShiftTypeId" });
            DropIndex("dbo.Shift", new[] { "ShiftPatternId" });
            DropIndex("dbo.Shift", new[] { "ShiftTemplateId" });
            DropIndex("dbo.ShiftPattern", new[] { "CalendarResourceRequirementId" });
            DropIndex("dbo.Calendar", new[] { "SubSiteId" });
            DropIndex("dbo.Calendar", new[] { "SiteId" });
            DropIndex("dbo.CalendarResourceRequirement", new[] { "ResourceId" });
            DropIndex("dbo.CalendarResourceRequirement", new[] { "CalendarId" });
            DropIndex("dbo.BudgetPeriod", new[] { "BudgetId" });
            DropIndex("dbo.User", new[] { "SystemAccessRoleId" });
            DropIndex("dbo.LeaveRequest", new[] { "AgencyWorkerId" });
            DropIndex("dbo.LeaveRequest", new[] { "EmployeeId" });
            DropIndex("dbo.PersonnelLeaveProfile", new[] { "AgencyWorkerId" });
            DropIndex("dbo.PersonnelLeaveProfile", new[] { "EmployeeId" });
            DropIndex("dbo.Contract", new[] { "AgencyWorkerId" });
            DropIndex("dbo.Contract", new[] { "EmployeeId" });
            DropIndex("dbo.AgencyWorker", new[] { "UserId" });
            DropIndex("dbo.AgencyWorker", new[] { "AgencyId" });
            DropTable("dbo.UnprocessedZkTimeData");
            DropTable("dbo.SubSiteType");
            DropTable("dbo.SubSite");
            DropTable("dbo.SiteType");
            DropTable("dbo.Site");
            DropTable("dbo.SitePersonnelLookup");
            DropTable("dbo.ShiftProfile");
            DropTable("dbo.ResourceRateModifier");
            DropTable("dbo.LeaveType");
            DropTable("dbo.Gender");
            DropTable("dbo.EmployeeType");
            DropTable("dbo.Employee");
            DropTable("dbo.Company");
            DropTable("dbo.ShiftType");
            DropTable("dbo.ShiftTemplate");
            DropTable("dbo.Shift");
            DropTable("dbo.ShiftPattern");
            DropTable("dbo.Resource");
            DropTable("dbo.Calendar");
            DropTable("dbo.CalendarResourceRequirement");
            DropTable("dbo.Budget");
            DropTable("dbo.BudgetPeriod");
            DropTable("dbo.AuditLog");
            DropTable("dbo.SystemAccessRole");
            DropTable("dbo.User");
            DropTable("dbo.LeaveRequest");
            DropTable("dbo.PersonnelLeaveProfile");
            DropTable("dbo.Contract");
            DropTable("dbo.AgencyWorker");
            DropTable("dbo.Agency");
        }
    }
}
