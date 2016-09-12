using System;
using System.Linq;
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
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto;
using RMS.AppServiceLayer.Users.Dto;
using RMS.AppServiceLayer.Zktime.Enums;
using RMS.AppServiceLayer.Zktime.Services;
using RMS.Core;

namespace RMS.AppServiceLayer.Helpers
{
    public class CommonHelperAppService
    {
        public static bool DeleteEntity()
        {
            return false;
        }

        public static string HashPassword(string password)
        {
            var saltedHash = PasswordHash.CreateHash(password);

            return saltedHash;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static long ReturnCalculatedTimespanBetweenTwoDateTimeObjects(DateTime clockInDateTime, DateTime clockOutDateTime, int status)
        {
            try
            {
                if (status == 5 || status == 6)
                {
                    // will return stupidly low or high integer
                    // so just return a 0 as time correction will
                    // update the field value in the end
                    return 0;
                }

                const int jsMillisecondDivider = 10000;
                var calculatedTime = clockOutDateTime - clockInDateTime;

                return (calculatedTime.Ticks) / jsMillisecondDivider;    
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public static TimeSpan ReturnConvertedTicksAsTimespan(long ticks)
        {
            return new TimeSpan(ticks);
        }

        public static bool ValidatePassword(string password, string hash)
        {
            var isValid = PasswordHash.ValidatePassword(password, hash);

            return isValid;
        }

        public static void MapDtoToEntityForUpdating(BudgetDto budgetDto, Budget budget)
        {
            budget.IsActive = budgetDto.IsActive;
            budget.StartDate = budgetDto.StartDate;
            budget.EndDate = budgetDto.EndDate;
            budget.ForecastTotal = budgetDto.ForecastTotal;
            budget.ActualTotal = budgetDto.ActualTotal;
            budget.SiteId = budgetDto.SiteId;
            budget.SubSiteId = budgetDto.SubSiteId;
        }

        public static void MapDtoToEntityForUpdating(BudgetPeriodDto budgetPeriodDto, BudgetPeriod budgetPeriod)
        {
            budgetPeriod.IsActive = budgetPeriodDto.IsActive;
            budgetPeriod.StartDate = budgetPeriodDto.StartDate;
            budgetPeriod.EndDate = budgetPeriodDto.EndDate;
            budgetPeriod.Amount = budgetPeriodDto.Amount;
            budgetPeriod.ForecastTotal = budgetPeriodDto.ForecastTotal;
            budgetPeriod.ActualSpendTotal = budgetPeriodDto.ActualSpendTotal;
        }

        public static void MapDtoToEntityForUpdating(CalendarDto calendarDto, Calendar calendar)
        {
            calendar.IsActive = calendarDto.IsActive;
            calendar.Name = calendarDto.Name;
            calendar.StartDate = calendarDto.StartDate;
            calendar.EndDate = calendarDto.EndDate;
            calendar.SiteId = calendarDto.SiteId;
            calendar.SubSiteId = calendarDto.SubSiteId;
        }

        public static void MapDtoToEntityForUpdating(CalendarResourceRequirementDto calendarResourceRequirementDto, CalendarResourceRequirement calendarResourceRequirement)
        {
            calendarResourceRequirement.IsActive = calendarResourceRequirementDto.IsActive;
            calendarResourceRequirement.CalendarId = calendarResourceRequirementDto.CalendarId;
            calendarResourceRequirement.StartDate = calendarResourceRequirementDto.StartDate;
            calendarResourceRequirement.EndDate = calendarResourceRequirementDto.EndDate;
        }

        public static void MapDtoToEntityForUpdating(CompanyDto companyDto, Company company)
        {
            company.IsActive = companyDto.IsActive;
            company.Name = companyDto.Name;
            company.Address = companyDto.Address;
            company.City = companyDto.City;
            company.Postcode = companyDto.Postcode;
            company.Telephone = companyDto.Telephone;
            company.Fax = companyDto.Fax;
            company.Email = companyDto.Email;
            company.ContactName = companyDto.ContactName;
        }

        public static void MapDtoToEntityForUpdating(ContractDto contractDto, Contract contract)
        {
            contract.IsActive = contractDto.IsActive;
            contract.ResourceId = contractDto.ResourceId;
            contract.EmployeeId = contractDto.EmployeeId;
            contract.WeeklyHours = contractDto.WeeklyHours;
            contract.BaseRateOverride = contractDto.BaseRateOverride;
            contract.OvertimeModifierOverride = contractDto.OvertimeModifierOverride;
            contract.StartDate = contractDto.StartDate;
            contract.EndDate = contractDto.EndDate;
        }

        public static void MapDtoToEntityForUpdating(EmployeeDto employeeDto, Employee employee)
        {
            employee.IsActive = employeeDto.IsActive;
            employee.Address = employeeDto.Address;
            employee.City = employeeDto.City;
            employee.Postcode = employeeDto.Postcode;
            employee.Telephone = employeeDto.Telephone;
            employee.Mobile = employeeDto.Mobile;
            employee.GenderId = employeeDto.GenderId;
            employee.CompanyId = employeeDto.CompanyId;
            employee.UserId = employeeDto.UserId;
            employee.EmployeeTypeId = employeeDto.EmployeeTypeId;
            employee.BaseSiteId = employeeDto.BaseSiteId;
            employee.BaseSubSiteId = employeeDto.BaseSubSiteId;
            employee.StartDate = employeeDto.StartDate;
            employee.LeaveDate = employeeDto.LeaveDate;
        }

        public static void MapDtoToEntityForUpdating(EmployeeTypeDto employeeTypeDto, EmployeeType employeeType)
        {
            employeeType.Name = employeeTypeDto.Name;
            employeeType.IsActive = employeeTypeDto.IsActive;
        }

        public static void MapDtoToEntityForUpdating(GenderDto genderDto, Gender gender)
        {
            gender.Name = genderDto.Name;
            gender.IsActive = genderDto.IsActive;
        }

        public static void MapDtoToEntityForUpdating(LeaveRequestDto leaveRequestDto, LeaveRequest leaveRequest)
        {
            leaveRequest.IsActive = leaveRequestDto.IsActive;
            leaveRequest.StartDateTime = leaveRequestDto.StartDateTime;
            leaveRequest.EndDateTime = leaveRequestDto.EndDateTime;
            leaveRequest.AmountRequested = leaveRequestDto.AmountRequested;
            leaveRequest.EmployeeId = leaveRequestDto.EmployeeId;
            leaveRequest.LeaveTypeId = leaveRequestDto.LeaveTypeId;
            leaveRequest.Notes = leaveRequestDto.Notes;
            leaveRequest.IsApproved = leaveRequestDto.IsApproved;
            leaveRequest.IsTaken = leaveRequestDto.IsTaken;
        }

        public static void MapDtoToEntityForUpdating(LeaveTypeDto leaveTypeDto, LeaveType leaveType)
        {
            leaveType.IsActive = leaveTypeDto.IsActive;
            leaveType.Name = leaveTypeDto.Name;
        }

        public static void MapDtoToEntityForUpdating(PersonnelLeaveProfileDto leaveProfileDto,
            PersonnelLeaveProfile leaveProfile)
        {
            leaveProfile.IsActive = leaveProfileDto.IsActive;
            leaveProfile.NumberOfDaysAllocated = leaveProfileDto.NumberOfDaysAllocated;
            leaveProfile.NumberOfDaysRemaining = leaveProfileDto.NumberOfDaysRemaining;
            leaveProfile.NumberOfDaysTaken = leaveProfileDto.NumberOfDaysTaken;
            leaveProfile.EmployeeId = leaveProfileDto.EmployeeId;
            leaveProfile.LeaveTypeId = leaveProfileDto.LeaveTypeId;
            leaveProfile.Notes = leaveProfileDto.Notes;
            leaveProfile.StartDate = leaveProfileDto.StartDate;
            leaveProfile.EndDate = leaveProfileDto.EndDate;
            leaveProfile.NumberOfHoursAllocated = leaveProfileDto.NumberOfHoursAllocated;
            leaveProfile.NumberOfHoursRemaining = leaveProfileDto.NumberOfHoursRemaining;
            leaveProfile.NumberOfHoursTaken = leaveProfileDto.NumberOfHoursTaken;
        }

        public static void MapDtoToEntityForUpdating(ResourceDto resourceDto, Resource resource)
        {
            resource.IsActive = resourceDto.IsActive;
            resource.Name = resourceDto.Name;
            resource.BaseRate = resourceDto.BaseRate;
        }

        public static void MapDtoToEntityForUpdating(ResourceRateModifierDto rateModifierDto, ResourceRateModifier rateModifier)
        {
            rateModifier.IsActive = rateModifierDto.IsActive;
            rateModifier.Name = rateModifierDto.Name;
            rateModifier.Value = rateModifierDto.Value;
            rateModifier.ShiftTypeId = rateModifierDto.ShiftTypeId;
            rateModifier.ResourceId = rateModifierDto.ResourceId;
        }

        public static void MapDtoToEntityForUpdating(ShiftDto shiftDto, Shift shift)
        {
            shift.IsActive = shiftDto.IsActive;
            shift.IsAssigned = shiftDto.IsAssigned;
            shift.ShiftTemplateId = shiftDto.ShiftTemplateId;
            shift.CalendarResourceRequirementId = shiftDto.CalendarResourceRequirementId;
            shift.EmployeeId = shiftDto.EmployeeId;
            shift.StartDate = shiftDto.StartDate;
            shift.EndDate = shiftDto.EndDate;
        }

        public static void MapDtoToEntityForUpdating(ShiftProfileDto shiftProfileDto,
            ShiftProfile shiftProfile)
        {
            shiftProfile.IsActive = shiftProfileDto.IsActive;
            shiftProfile.StartDateTime = shiftProfileDto.StartDateTime;
            shiftProfile.EndDateTime = shiftProfileDto.EndDateTime;
            shiftProfile.ActualStartDateTime = shiftProfileDto.ActualStartDateTime;
            shiftProfile.ActualEndDateTime = shiftProfileDto.ActualEndDateTime;
            shiftProfile.ZktStartDateTime = shiftProfileDto.ZktStartDateTime;
            shiftProfile.ZktEndDateTime = shiftProfileDto.ZktEndDateTime;
            shiftProfile.HoursWorked = shiftProfileDto.HoursWorked;
            shiftProfile.EmployeeId = shiftProfileDto.EmployeeId;
            shiftProfile.ShiftId = shiftProfileDto.ShiftId;
            shiftProfile.IsApproved = shiftProfileDto.IsApproved;
            shiftProfile.Status = shiftProfileDto.Status;
            shiftProfile.Reason = shiftProfileDto.Reason;
            shiftProfile.Notes = shiftProfileDto.Notes;
            shiftProfile.IsModified = shiftProfileDto.IsModified;
        }

        public static void MapDtoToEntityForUpdating(ShiftTemplateDto shiftTemplateDto, ShiftTemplate shiftTemplate)
        {
            shiftTemplate.IsActive = shiftTemplateDto.IsActive;
            shiftTemplate.Name = shiftTemplateDto.Name;
            shiftTemplate.ShiftRate = shiftTemplateDto.ShiftRate;
            shiftTemplate.ShiftTypeId = shiftTemplateDto.ShiftTypeId;
            shiftTemplate.ResourceId = shiftTemplateDto.ResourceId;
            shiftTemplate.SiteId = shiftTemplateDto.SiteId;
            shiftTemplate.SubSiteId = shiftTemplateDto.SubSiteId;
            shiftTemplate.StartTime = shiftTemplateDto.StartTime;
            shiftTemplate.EndTime = shiftTemplateDto.EndTime;
            shiftTemplate.Duration = shiftTemplateDto.Duration;
            shiftTemplate.UnpaidBreakDuration = shiftTemplateDto.UnpaidBreakDuration;
            
            shiftTemplate.Mon = shiftTemplateDto.Mon;
            shiftTemplate.Tue = shiftTemplateDto.Tue;
            shiftTemplate.Wed = shiftTemplateDto.Wed;
            shiftTemplate.Thu = shiftTemplateDto.Thu;
            shiftTemplate.Fri = shiftTemplateDto.Fri;
            shiftTemplate.Sat = shiftTemplateDto.Sat;
            shiftTemplate.Sun = shiftTemplateDto.Sun;
        }

        public static void MapDtoToEntityForUpdating(ShiftTypeDto shiftTypeDto, ShiftType shiftType)
        {
            shiftType.IsActive = shiftTypeDto.IsActive;
            shiftType.Name = shiftTypeDto.Name;
            shiftType.IsOvernight = shiftTypeDto.IsOvernight;
        }

        public static void MapDtoToEntityForUpdating(SiteDto siteDto, Site site)
        {
            site.IsActive = siteDto.IsActive;
            site.Name = siteDto.Name;
            site.PayrollStartDate = siteDto.PayrollStartDate;
            site.PayrollEndDate = siteDto.PayrollEndDate;
            site.SiteTypeId = siteDto.SiteTypeId;
            site.CompanyId = siteDto.CompanyId;
        }

        public static void MapDtoToEntityForUpdating(SiteTypeDto siteTypeDto, SiteType siteType)
        {
            siteType.Name = siteTypeDto.Name;
            siteType.IsActive = siteTypeDto.IsActive;
        }

        public static void MapDtoToEntityForUpdating(SubSiteDto subSiteDto, SubSite subSite)
        {
            subSite.IsActive = subSiteDto.IsActive;
            subSite.Name = subSiteDto.Name;
            subSite.SiteId = subSiteDto.SiteId;
            subSite.SubSiteTypeId = subSiteDto.SubSiteTypeId;
        }

        public static void MapDtoToEntityForUpdating(SubSiteTypeDto subSiteTypeDto, SubSiteType subSiteType)
        {
            subSiteType.Name = subSiteTypeDto.Name;
            subSiteType.IsActive = subSiteTypeDto.IsActive;
        }

        public static void MapDtoToEntityForUpdating(SitePersonnelLookupDto sitePersonnelLookupDto, SitePersonnelLookup sitePersonnelLookup)
        {
            sitePersonnelLookup.IsActive = sitePersonnelLookupDto.IsActive;
            sitePersonnelLookup.EmployeeId = sitePersonnelLookupDto.EmployeeId;
            sitePersonnelLookup.SiteId = sitePersonnelLookupDto.SiteId;            
            sitePersonnelLookup.SubSiteId = sitePersonnelLookupDto.SubSiteId;
        }

        public static void MapDtoToEntityForUpdating(SystemAccessRoleDto systemAccessRoleDto, SystemAccessRole systemAccessRole)
        {
            systemAccessRole.Name = systemAccessRoleDto.Name;
            systemAccessRole.IsActive = systemAccessRoleDto.IsActive;
        }

        public static void MapDtoToEntityForUpdating(TimeAdjustmentFormDto timeAdjustmentFormDto, TimeAdjustmentForm timeAdjustmentForm)
        {
            timeAdjustmentForm.IsActive = timeAdjustmentFormDto.IsActive;
            timeAdjustmentForm.ActualEndDateTime = timeAdjustmentFormDto.ActualEndDateTime;
            timeAdjustmentForm.ActualStartDateTime = timeAdjustmentFormDto.ActualStartDateTime;
            timeAdjustmentForm.ZktStartDateTime = timeAdjustmentFormDto.ZktStartDateTime;
            timeAdjustmentForm.ZktEndDateTime = timeAdjustmentFormDto.ZktEndDateTime;
            timeAdjustmentForm.EarlyOut = timeAdjustmentFormDto.EarlyOut;
            timeAdjustmentForm.LateIn = timeAdjustmentFormDto.LateIn;
            timeAdjustmentForm.MissedClockIn = timeAdjustmentFormDto.MissedClockIn;
            timeAdjustmentForm.MissedClockOut = timeAdjustmentFormDto.MissedClockOut;
            timeAdjustmentForm.Notes = timeAdjustmentFormDto.Notes;
            timeAdjustmentForm.ShiftEndDateTime = timeAdjustmentFormDto.ShiftEndDateTime;
            timeAdjustmentForm.ShiftLocation = timeAdjustmentFormDto.ShiftLocation;
            timeAdjustmentForm.ShiftStartDateTime = timeAdjustmentFormDto.ShiftStartDateTime;
            timeAdjustmentForm.StaffName = timeAdjustmentFormDto.StaffName;
            timeAdjustmentForm.EmployeeId = timeAdjustmentFormDto.EmployeeId;
            timeAdjustmentForm.ShiftId = timeAdjustmentFormDto.ShiftId;
            timeAdjustmentForm.ShiftProfileId = timeAdjustmentFormDto.ShiftProfileId;
            timeAdjustmentForm.IsManagerApproved = timeAdjustmentFormDto.IsManagerApproved;
            timeAdjustmentForm.IsAdminApproved = timeAdjustmentFormDto.IsAdminApproved;
        }

        public static void MapDtoToEntityForUpdating(UserDefinedSystemConfigDto udscDto, UserDefinedSystemConfig udsc)
        {
            udsc.NHoursAgo = udscDto.NHoursAgo;
            udsc.ShiftPostStartStillValidThresholdValue = udscDto.ShiftPostStartStillValidThresholdValue;
            udsc.ShiftPreStartEarlyInThresholdValue = udscDto.ShiftPreStartEarlyInThresholdValue;
            udsc.ShiftPostEndValidThresholdValue = udscDto.ShiftPostEndValidThresholdValue;
            udsc.PayrollStartDayOfMonth = udscDto.PayrollStartDayOfMonth;
            udsc.PayrollEndDayOfMonth = udscDto.PayrollEndDayOfMonth;
            udsc.NICFactor = udscDto.NICFactor;
            udsc.AccruedHolidayFactor = udscDto.AccruedHolidayFactor;
        }

        public static void MapDtoToEntityForUpdating(UserDto userDto, User user)
        {
            user.IsActive = userDto.IsActive;
            user.Login = userDto.Login;
            user.Password = userDto.Password;
            user.Email = userDto.Email;
            user.Firstname = userDto.Firstname;
            user.Lastname = userDto.Lastname;
            user.SystemAccessRoleId = userDto.SystemAccessRoleId;
            user.IsAccountLocked = userDto.IsAccountLocked;
            user.ExternalTimeSystemId = userDto.ExternalTimeSystemId;
            user.PayrollReferenceNumber = userDto.PayrollReferenceNumber;
        }
    }
}
