using AutoMapper;
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
using RMS.AppServiceLayer.Reports.Dto;
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
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto;
using RMS.AppServiceLayer.Users.Dto;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI
{
    public class AutoMapperWebApiConfig
    {
        public static void RegisterMappings()
        {
            // Model to DTO
            Mapper.CreateMap<BudgetModel, BudgetDto>();

            Mapper.CreateMap<BudgetPeriodModel, BudgetPeriodDto>();

            Mapper.CreateMap<CalendarModel, CalendarDto>();

            Mapper.CreateMap<CalendarResourceRequirementModel, CalendarResourceRequirementDto>();

            Mapper.CreateMap<CompanyModel, CompanyDto>();

            Mapper.CreateMap<ContractModel, ContractDto>();

            Mapper.CreateMap<EmployeeModel, EmployeeDto>()
                .ForMember(e => e.ContractDtos, c => c.MapFrom(m => m.ContractModels));
            
            Mapper.CreateMap<EmployeeTypeModel, EmployeeTypeDto>();
            
            Mapper.CreateMap<GenderModel, GenderDto>();

            Mapper.CreateMap<LeaveRequestModel, LeaveRequestDto>();

            Mapper.CreateMap<LeaveTypeModel, LeaveTypeDto>();

            Mapper.CreateMap<PersonnelLeaveProfileModel, PersonnelLeaveProfileDto>();

            Mapper.CreateMap<ResourceModel, ResourceDto>();

            Mapper.CreateMap<ResourceRateModifierModel, ResourceRateModifierDto>();

            Mapper.CreateMap<ShiftModel, ShiftDto>();

            Mapper.CreateMap<ShiftProfileModel, ShiftProfileDto>();

            Mapper.CreateMap<ShiftTemplateModel, ShiftTemplateDto>();

            Mapper.CreateMap<ShiftTypeModel, ShiftTypeDto>();

            Mapper.CreateMap<SiteModel, SiteDto>();

            Mapper.CreateMap<SiteTypeModel, SiteTypeDto>();

            Mapper.CreateMap<SubSiteModel, SubSiteDto>();

            Mapper.CreateMap<SubSiteTypeModel, SubSiteTypeDto>();

            Mapper.CreateMap<SitePersonnelLookupModel, SitePersonnelLookupDto>();

            Mapper.CreateMap<SystemAccessRoleModel, SystemAccessRoleDto>();

            Mapper.CreateMap<TimeAdjustmentFormModel, TimeAdjustmentFormDto>();

            Mapper.CreateMap<UserDefinedSystemConfigModel, UserDefinedSystemConfigDto>();
            
            Mapper.CreateMap<UserModel, UserDto>();


            // Model mapping for reporting
            Mapper.CreateMap<DailyActivityReportParamsModel, DailyActivityReportParamsDto>();
            Mapper.CreateMap<MonthlyPayrollReportParamsModel, MonthlyPayrollReportParamsDto>();


            // DTO to Model
            Mapper.CreateMap<BudgetDto, BudgetModel>()
                .ForMember(b => b.BudgetPeriodModels, bpm => bpm.MapFrom(m => m.BudgetPeriodDtos));

            Mapper.CreateMap<BudgetPeriodDto, BudgetPeriodModel>();

            Mapper.CreateMap<CalendarDto, CalendarModel>();

            Mapper.CreateMap<CalendarResourceRequirementDto, CalendarResourceRequirementModel>();

            Mapper.CreateMap<CompanyDto, CompanyModel>()
                .ForMember(c => c.EmployeeModels, e => e.MapFrom(m => m.EmployeeDtos));

            Mapper.CreateMap<ContractDto, ContractModel>();

            Mapper.CreateMap<EmployeeDto, EmployeeModel>()
                .ForMember(e => e.UserModel, u => u.MapFrom(m => m.UserDto))
                .ForMember(e => e.ContractModels, c => c.MapFrom(m => m.ContractDtos))
                .ForMember(e => e.LeaveRequestModels, lr => lr.MapFrom(m => m.LeaveRequestDtos))
                .ForMember(e => e.LeaveProfileModels, lp => lp.MapFrom(m => m.LeaveProfileDtos));
            
            Mapper.CreateMap<EmployeeTypeDto, EmployeeTypeModel>();
            
            Mapper.CreateMap<GenderDto, GenderModel>();

            Mapper.CreateMap<LeaveRequestDto, LeaveRequestModel>();

            Mapper.CreateMap<LeaveTypeDto, LeaveTypeModel>();

            Mapper.CreateMap<PersonnelLeaveProfileDto, PersonnelLeaveProfileModel>();

            Mapper.CreateMap<ResourceDto, ResourceModel>();

            Mapper.CreateMap<ResourceRateModifierDto, ResourceRateModifierModel>();

            Mapper.CreateMap<ShiftDto, ShiftModel>()
                .ForMember(s => s.ShiftTemplateModel, st => st.MapFrom(m => m.ShiftTemplateDto));

            Mapper.CreateMap<ShiftProfileDto, ShiftProfileModel>()
                .ForMember(sp => sp.ShiftModel, s => s.MapFrom(m => m.ShiftDto));

            Mapper.CreateMap<ShiftTemplateDto, ShiftTemplateModel>()
                .ForMember(st => st.ShiftTypeModel, stm => stm.MapFrom(m => m.ShiftTypeDto));

            Mapper.CreateMap<ShiftTypeDto, ShiftTypeModel>();

            Mapper.CreateMap<SiteDto, SiteModel>()
                .ForMember(s => s.SubSiteModels, ss => ss.MapFrom(m => m.SubSiteDtos))
                .ForMember(s => s.CalendarModels, sc => sc.MapFrom(m => m.CalendarDtos));

            Mapper.CreateMap<SiteTypeDto, SiteTypeModel>();

            Mapper.CreateMap<SubSiteDto, SubSiteModel>()
                .ForMember(ss => ss.CalendarModels, ssc => ssc.MapFrom(m => m.CalendarDtos));

            Mapper.CreateMap<SubSiteTypeDto, SubSiteTypeModel>();

            Mapper.CreateMap<SitePersonnelLookupDto, SitePersonnelLookupModel>();

            Mapper.CreateMap<SystemAccessRoleDto, SystemAccessRoleModel>();

            Mapper.CreateMap<TimeAdjustmentFormDto, TimeAdjustmentFormModel>();

            Mapper.CreateMap<UserDefinedSystemConfigDto, UserDefinedSystemConfigModel>();

            Mapper.CreateMap<UserDto, UserModel>()
                .ForMember(u => u.SystemAccessRoleModel, s => s.MapFrom(m => m.SystemAccessRoleDto));
        }
    }
}