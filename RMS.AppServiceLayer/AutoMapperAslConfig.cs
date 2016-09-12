using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Dto;
using RMS.AppServiceLayer.BudgetPeriods.Dto;
using RMS.AppServiceLayer.Budgets.Dto;
using RMS.AppServiceLayer.CalendarResourceRequirements.Dto;
using RMS.AppServiceLayer.Calendars.Dto;
using RMS.AppServiceLayer.Companies.Dto;
using RMS.AppServiceLayer.Contracts.Dto;
using RMS.AppServiceLayer.Employees.Dto;
using RMS.AppServiceLayer.EmployeeTypes.Dto;
using RMS.AppServiceLayer.Genders.Dto;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.LeaveTypes.Dto;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Dto;
using RMS.AppServiceLayer.ResourceRateModifiers.Dto;
using RMS.AppServiceLayer.Resources.Dto;
using RMS.AppServiceLayer.ShiftProfiles.Dto;
using RMS.AppServiceLayer.Shifts.Dto;
using RMS.AppServiceLayer.ShiftTemplates.Dto;
using RMS.AppServiceLayer.ShiftTypes.Dto;
using RMS.AppServiceLayer.SitePersonnelLookups.Dto;
using RMS.AppServiceLayer.Sites.Dto;
using RMS.AppServiceLayer.SiteTypes.Dto;
using RMS.AppServiceLayer.SubSites.Dto;
using RMS.AppServiceLayer.SubSiteTypes.Dto;
using RMS.AppServiceLayer.SystemAccessRoles.Dto;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;
using RMS.AppServiceLayer.UnprocessedZkTimeData.Dto;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto;
using RMS.AppServiceLayer.Users.Dto;
using RMS.Core;

namespace RMS.AppServiceLayer
{
    public class AutoMapperAslConfig
    {
        public static void RegisterMappings()
        {
            // DTO to Entity
            Mapper.CreateMap<AuditLogDto, AuditLog>();
            
            Mapper.CreateMap<BudgetDto, Budget>();

            Mapper.CreateMap<BudgetPeriodDto, BudgetPeriod>();

            Mapper.CreateMap<CalendarDto, Calendar>();

            Mapper.CreateMap<CalendarResourceRequirementDto, CalendarResourceRequirement>();

            Mapper.CreateMap<CompanyDto, Company>();

            Mapper.CreateMap<ContractDto, Contract>();
            
            Mapper.CreateMap<EmployeeDto, Employee>()
                .ForMember(e => e.Contracts, c => c.MapFrom(m => m.ContractDtos));
            
            Mapper.CreateMap<EmployeeTypeDto, EmployeeType>();
            
            Mapper.CreateMap<GenderDto, Gender>();

            Mapper.CreateMap<LeaveRequestDto, LeaveRequest>();

            Mapper.CreateMap<LeaveTypeDto, LeaveType>();

            Mapper.CreateMap<PersonnelLeaveProfileDto, PersonnelLeaveProfile>();

            Mapper.CreateMap<ResourceDto, Resource>();

            Mapper.CreateMap<ResourceRateModifierDto, ResourceRateModifier>();

            Mapper.CreateMap<ShiftDto, Shift>();

            Mapper.CreateMap<ShiftProfileDto, ShiftProfile>();

            Mapper.CreateMap<ShiftTemplateDto, ShiftTemplate>();

            Mapper.CreateMap<ShiftTypeDto, ShiftType>();

            Mapper.CreateMap<SiteDto, Site>();
            
            Mapper.CreateMap<SiteTypeDto, SiteType>();

            Mapper.CreateMap<SubSiteDto, SubSite>();

            Mapper.CreateMap<SubSiteTypeDto, SubSiteType>();

            Mapper.CreateMap<SitePersonnelLookupDto, SitePersonnelLookup>();

            Mapper.CreateMap<SystemAccessRoleDto, SystemAccessRole>();

            Mapper.CreateMap<TimeAdjustmentFormDto, TimeAdjustmentForm>();

            Mapper.CreateMap<UserDefinedSystemConfigDto, UserDefinedSystemConfig>();

            Mapper.CreateMap<UserDto, User>();

            Mapper.CreateMap<UnprocessedZkTimeDataDto, ZkTimeClockingRecord>();


            // Entity to DTO
            Mapper.CreateMap<AuditLog, AuditLogDto>();
            
            Mapper.CreateMap<Budget, BudgetDto>()
                .ForMember(b => b.BudgetPeriodDtos, bp => bp.MapFrom(m => m.BudgetPeriods));

            Mapper.CreateMap<BudgetPeriod, BudgetPeriodDto>();

            Mapper.CreateMap<Calendar, CalendarDto>();

            Mapper.CreateMap<CalendarResourceRequirement, CalendarResourceRequirementDto>();

            Mapper.CreateMap<Company, CompanyDto>()
                .ForMember(c => c.EmployeeDtos, e => e.MapFrom(m => m.Employees));

            Mapper.CreateMap<Contract, ContractDto>();

            Mapper.CreateMap<Employee, EmployeeDto>()
                .ForMember(e => e.UserDto, u => u.MapFrom(m => m.User))
                .ForMember(e => e.ContractDtos, c => c.MapFrom(m => m.Contracts))
                .ForMember(e => e.LeaveRequestDtos, lr => lr.MapFrom(m => m.LeaveRequests))
                .ForMember(e => e.LeaveProfileDtos, lp => lp.MapFrom(m => m.LeaveProfiles));
            
            Mapper.CreateMap<EmployeeType, EmployeeTypeDto>();
            
            Mapper.CreateMap<Gender, GenderDto>();

            Mapper.CreateMap<LeaveRequest, LeaveRequestDto>();

            Mapper.CreateMap<LeaveType, LeaveTypeDto>();

            Mapper.CreateMap<PersonnelLeaveProfile, PersonnelLeaveProfileDto>();

            Mapper.CreateMap<Resource, ResourceDto>();

            Mapper.CreateMap<ResourceRateModifier, ResourceRateModifierDto>();

            Mapper.CreateMap<Shift, ShiftDto>()
                .ForMember(s => s.ShiftTemplateDto, st => st.MapFrom(m => m.ShiftTemplate));

            Mapper.CreateMap<ShiftProfile, ShiftProfileDto>()
                .ForMember(sp => sp.ShiftDto, s => s.MapFrom(m => m.Shift));

            Mapper.CreateMap<ShiftType, ShiftTypeDto>();

            Mapper.CreateMap<Site, SiteDto>()
                .ForMember(s => s.SubSiteDtos, ss => ss.MapFrom(m => m.SubSites))
                .ForMember(s => s.CalendarDtos, sc => sc.MapFrom(m => m.Calendars));

            Mapper.CreateMap<ShiftTemplate, ShiftTemplateDto>()
                .ForMember(st => st.ShiftTypeDto, std => std.MapFrom(m => m.ShiftType));

            Mapper.CreateMap<SiteType, SiteTypeDto>();

            Mapper.CreateMap<SubSite, SubSiteDto>()
                .ForMember(ss => ss.CalendarDtos, ssc => ssc.MapFrom(m => m.Calendars));

            Mapper.CreateMap<SubSiteType, SubSiteTypeDto>();

            Mapper.CreateMap<SitePersonnelLookup, SitePersonnelLookupDto>();

            Mapper.CreateMap<SystemAccessRole, SystemAccessRoleDto>();

            Mapper.CreateMap<TimeAdjustmentForm, TimeAdjustmentFormDto>();

            Mapper.CreateMap<UserDefinedSystemConfig, UserDefinedSystemConfigDto>();

            Mapper.CreateMap<User, UserDto>()
                .ForMember(u => u.SystemAccessRoleDto, s => s.MapFrom(m => m.SystemAccessRole));
        }
    }
}
