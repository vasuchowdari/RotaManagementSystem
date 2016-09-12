using Microsoft.Practices.Unity;
using System.Web.Http;
using RMS.AppServiceLayer.Accounts.Interfaces;
using RMS.AppServiceLayer.Accounts.Services;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.AuditLogs.Services;
using RMS.AppServiceLayer.BudgetPeriods.Interfaces;
using RMS.AppServiceLayer.BudgetPeriods.Services;
using RMS.AppServiceLayer.Budgets.Interfaces;
using RMS.AppServiceLayer.Budgets.Services;
using RMS.AppServiceLayer.CalendarResourceRequirements.Interfaces;
using RMS.AppServiceLayer.CalendarResourceRequirements.Services;
using RMS.AppServiceLayer.Calendars.Interfaces;
using RMS.AppServiceLayer.Calendars.Services;
using RMS.AppServiceLayer.Contracts.Interfaces;
using RMS.AppServiceLayer.Contracts.Services;
using RMS.AppServiceLayer.Employees.Interfaces;
using RMS.AppServiceLayer.Employees.Services;
using RMS.AppServiceLayer.EmployeeTypes.Interfaces;
using RMS.AppServiceLayer.EmployeeTypes.Services;
using RMS.AppServiceLayer.Companies.Interfaces;
using RMS.AppServiceLayer.Companies.Services;
using RMS.AppServiceLayer.Genders.Interfaces;
using RMS.AppServiceLayer.Genders.Services;
using RMS.AppServiceLayer.LeaveRequests.Interfaces;
using RMS.AppServiceLayer.LeaveRequests.Services;
using RMS.AppServiceLayer.LeaveTypes.Interfaces;
using RMS.AppServiceLayer.LeaveTypes.Services;
using RMS.AppServiceLayer.Mailers.Interfaces;
using RMS.AppServiceLayer.Mailers.Services;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Interfaces;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Services;
using RMS.AppServiceLayer.Reports.Interfaces;
using RMS.AppServiceLayer.Reports.Services;
using RMS.AppServiceLayer.ResourceRateModifiers.Interfaces;
using RMS.AppServiceLayer.ResourceRateModifiers.Services;
using RMS.AppServiceLayer.Resources.Interfaces;
using RMS.AppServiceLayer.Resources.Services;
using RMS.AppServiceLayer.ShiftProfiles.Interfaces;
using RMS.AppServiceLayer.ShiftProfiles.Services;
using RMS.AppServiceLayer.Shifts.Interfaces;
using RMS.AppServiceLayer.Shifts.Services;
using RMS.AppServiceLayer.ShiftTemplates.Interfaces;
using RMS.AppServiceLayer.ShiftTemplates.Services;
using RMS.AppServiceLayer.ShiftTypes.Interfaces;
using RMS.AppServiceLayer.ShiftTypes.Services;
using RMS.AppServiceLayer.SitePersonnelLookups.Interfaces;
using RMS.AppServiceLayer.SitePersonnelLookups.Services;
using RMS.AppServiceLayer.Sites.Interfaces;
using RMS.AppServiceLayer.Sites.Services;
using RMS.AppServiceLayer.SiteTypes.Interfaces;
using RMS.AppServiceLayer.SiteTypes.Services;
using RMS.AppServiceLayer.SubSites.Interfaces;
using RMS.AppServiceLayer.SubSites.Services;
using RMS.AppServiceLayer.SubSiteTypes.Interfaces;
using RMS.AppServiceLayer.SubSiteTypes.Services;
using RMS.AppServiceLayer.SystemAccessRoles.Interfaces;
using RMS.AppServiceLayer.SystemAccessRoles.Services;
using RMS.AppServiceLayer.TimeFormAdjustments.Interfaces;
using RMS.AppServiceLayer.TimeFormAdjustments.Services;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Interfaces;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Services;
using RMS.AppServiceLayer.Users.Interfaces;
using RMS.AppServiceLayer.Users.Services;
using RMS.AppServiceLayer.Zktime.Interfaces;
using RMS.AppServiceLayer.Zktime.Services;
using RMS.Zktime.Interfaces;
using RMS.Zktime.Services;
using Unity.WebApi;

namespace RMS.WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IAccountAppService, AccountAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuditLogAppService, AuditLogAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBudgetAppService, BudgetAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBudgetPeriodAppService, BudgetPeriodAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICalendarAppService, CalendarAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICalResourceRqAppService, CalResourceRqAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompanyAppService, CompanyAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContractAppService, ContractAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeeAppService, EmployeeAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeeTypeAppService, EmployeeTypeAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGenderAppService, GenderAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeaveRequestAppService, LeaveRequestAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeaveTypeAppService, LeaveTypeAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMailerAppService, MailerAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPersonnelLeaveProfileAppService, PersonnelLeaveProfileAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IResourceAppService, ResourceAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IResourceRateModifierAppService, ResourceRateModifierAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportAppService, ReportAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IShiftAppService, ShiftAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IShiftProfileAppService, ShiftProfileAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IShiftTemplateAppService, ShiftTemplateAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IShiftTypeAppService, ShiftTypeAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISiteAppService, SiteAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISiteTypeAppService, SiteTypeAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISubSiteAppService, SubSiteAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISubSiteTypeAppService, SubSiteTypeAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISitePersonnelLookupAppService, SitePersonnelLookupAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISystemAccessRoleAppService, SystemAccessRoleAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITimeAdjustmentFormAppService, TimeAdjustmentFormAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserDefinedSystemConfigAppService, UserDefinedSystemConfigAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserAppService, UserAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IZktRecordAppService, ZktRecordAppService>(new HierarchicalLifetimeManager());
            container.RegisterType<IZkTimeService, ZkTimeService>(new HierarchicalLifetimeManager());

            container.RegisterType<IZkTimeModule, ZkTimeModule>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}