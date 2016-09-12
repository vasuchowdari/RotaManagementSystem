using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RMS.Core;

namespace RMS.Infrastructure
{
    public class RmsContext : DbContext
    {
        public RmsContext() 
            : base("RmsContext")
        {
            
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetPeriod> BudgetPeriods { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarResourceRequirement> CalendarResourceRequirements { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<PersonnelLeaveProfile> PersonnelLeaveProfiles { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceRateModifier> ResourceRateModifiers { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftProfile> ShiftProfiles { get; set; }
        public DbSet<ShiftTemplate> ShiftTemplates { get; set; }
        public DbSet<ShiftType> ShiftTypes { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteType> SiteTypes { get; set; }
        public DbSet<SubSite> SubSites { get; set; }
        public DbSet<SubSiteType> SubSiteTypes { get; set; }
        public DbSet<SitePersonnelLookup> SitePersonnelLookups { get; set; }
        public DbSet<SystemAccessRole> SystemAccessRoles { get; set; }
        public DbSet<TimeAdjustmentForm> TimeAdjustmentForms { get; set; }
        public DbSet<UserDefinedSystemConfig> UserDefinedSystemConfigs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ZkTimeClockingRecord> ZkTimeClockingRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
