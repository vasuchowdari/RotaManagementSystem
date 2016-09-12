using System;
using Moq;
using NUnit.Framework;
using RMS.AppServiceLayer.BudgetPeriods.Dto;
using RMS.AppServiceLayer.Budgets.Dto;
using RMS.AppServiceLayer.CalendarResourceRequirements.Dto;
using RMS.AppServiceLayer.Calendars.Dto;
using RMS.AppServiceLayer.Companies.Dto;
using RMS.AppServiceLayer.Contracts.Dto;
using RMS.AppServiceLayer.Employees.Dto;
using RMS.AppServiceLayer.EmployeeTypes.Dto;
using RMS.AppServiceLayer.Genders.Dto;
using RMS.AppServiceLayer.Helpers;
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
using RMS.AppServiceLayer.Users.Dto;
using RMS.Core;

namespace RMS.ServerTests.AppServiceLayer.Helpers
{
    [TestFixture]
    public class CommonHelperTests : IDisposable
    {
        private string _testAddressString;
        private string _testCityString;
        private string _testPostcodeString;
        private string _testTelephoneString;
        private string _testFaxString;
        private string _testMobileString;
        private string _testNameString;
        private string _testEmailString;
        private DateTime _testDateTime;

        [OneTimeSetUp]
        public void Init()
        {
            _testAddressString = "Address";
            _testCityString = "City";
            _testPostcodeString = "HD5 8RR";
            _testTelephoneString = "01484 123 456";
            _testFaxString = "01462 654 321";
            _testMobileString = "07977 905 547";
            _testNameString = "Name";
            _testEmailString = "test@email.com";
            _testDateTime = DateTime.UtcNow;
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            
        }

        [Test]
        public void Should_Map_BudgetDto_to_Budget()
        {
            // Arrange
            var _mBudget = new Mock<Budget>();

            var budgetDto = new BudgetDto
            {
                IsActive = true,
                ActualTotal = 100,
                ForecastTotal = 120,
                StartDate = _testDateTime,
                EndDate = _testDateTime,
                SiteId = 1,
                SubSiteId = null
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(budgetDto, _mBudget.Object);

            // Assert
            Assert.AreEqual(budgetDto.IsActive, _mBudget.Object.IsActive);
            Assert.AreEqual(budgetDto.ActualTotal, _mBudget.Object.ActualTotal);
            Assert.AreEqual(budgetDto.ForecastTotal, _mBudget.Object.ForecastTotal);
            Assert.AreEqual(budgetDto.StartDate, _mBudget.Object.StartDate);
            Assert.AreEqual(budgetDto.EndDate, _mBudget.Object.EndDate);
            Assert.AreEqual(budgetDto.SiteId, _mBudget.Object.SiteId);
            Assert.AreEqual(budgetDto.SubSiteId, _mBudget.Object.SubSiteId);
        }

        [Test]
        public void Should_Map_BudgetPeriodDto_to_BudgetPeriod()
        {
            // Arrange
            var _mBudgetPeriod = new Mock<BudgetPeriod>();

            var budgetPeriodDto = new BudgetPeriodDto
            {
                IsActive = true,
                StartDate = _testDateTime,
                EndDate = _testDateTime,
                ActualSpendTotal = 1000,
                Amount = 1001,
                ForecastTotal = 1200
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(budgetPeriodDto, _mBudgetPeriod.Object);

            // Assert
            Assert.AreEqual(budgetPeriodDto.IsActive, _mBudgetPeriod.Object.IsActive);
            Assert.AreEqual(budgetPeriodDto.StartDate, _mBudgetPeriod.Object.StartDate);
            Assert.AreEqual(budgetPeriodDto.EndDate, _mBudgetPeriod.Object.EndDate);
            Assert.AreEqual(budgetPeriodDto.ActualSpendTotal, _mBudgetPeriod.Object.ActualSpendTotal);
            Assert.AreEqual(budgetPeriodDto.Amount, _mBudgetPeriod.Object.Amount);
            Assert.AreEqual(budgetPeriodDto.ForecastTotal, _mBudgetPeriod.Object.ForecastTotal);
        }

        [Test]
        public void Should_Map_CalendarDto_to_Calendar()
        {
            // Arrange
            var _mCalendar = new Mock<Calendar>();

            var calendarDto = new CalendarDto
            {
                IsActive = true,
                StartDate = _testDateTime,
                EndDate = _testDateTime,
                Name = _testNameString,
                SiteId = 1,
                SubSiteId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(calendarDto, _mCalendar.Object);

            // Assert
            Assert.AreEqual(calendarDto.IsActive, _mCalendar.Object.IsActive);
            Assert.AreEqual(calendarDto.StartDate, _mCalendar.Object.StartDate);
            Assert.AreEqual(calendarDto.EndDate, _mCalendar.Object.EndDate);
            Assert.AreEqual(calendarDto.Name, _mCalendar.Object.Name);
            Assert.AreEqual(calendarDto.SiteId, _mCalendar.Object.SiteId);
            Assert.AreEqual(calendarDto.SubSiteId, _mCalendar.Object.SubSiteId);
        }

        [Test]
        public void Should_map_CalendarResourceRequirementDto_to_CalendarResourceRequirement()
        {
            // Arrange
            var _mCalResRq = new Mock<CalendarResourceRequirement>();

            var calResRqDto = new CalendarResourceRequirementDto
            {
                IsActive = true,
                CalendarId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(calResRqDto, _mCalResRq.Object);

            // Assert
            Assert.AreEqual(calResRqDto.IsActive, _mCalResRq.Object.IsActive);
            Assert.AreEqual(calResRqDto.CalendarId, _mCalResRq.Object.CalendarId);
        }

        [Test]
        public void Should_Map_CompanyDto_to_Company()
        {
            // Arrange
            var _mCompany = new Mock<Company>();

            var companyDto = new CompanyDto
            {
                IsActive = true,
                Address = _testAddressString,
                City = _testCityString,
                Postcode = _testPostcodeString,
                Telephone = _testTelephoneString,
                Fax = _testFaxString,
                ContactName = _testNameString,
                Email = _testEmailString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(companyDto, _mCompany.Object);

            // Assert
            Assert.AreEqual(companyDto.IsActive, _mCompany.Object.IsActive);
            Assert.AreEqual(companyDto.Address, _mCompany.Object.Address);
            Assert.AreEqual(companyDto.City, _mCompany.Object.City);
            Assert.AreEqual(companyDto.Postcode, _mCompany.Object.Postcode);
            Assert.AreEqual(companyDto.Telephone, _mCompany.Object.Telephone);
            Assert.AreEqual(companyDto.Fax, _mCompany.Object.Fax);
            Assert.AreEqual(companyDto.ContactName, _mCompany.Object.ContactName);
            Assert.AreEqual(companyDto.Email, _mCompany.Object.Email);
        }

        [Test]
        public void Should_Map_ContractDto_to_Contract()
        {
            // Arrange
            var _mContract = new Mock<Contract>();

            var contractDto = new ContractDto
            {
                IsActive = true,
                EmployeeId = null,
                BaseRateOverride = 1.25,
                OvertimeModifierOverride = 2,
                ResourceId = 1,
                WeeklyHours = 40.5
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(contractDto, _mContract.Object);

            // Assert
            Assert.AreEqual(contractDto.IsActive, _mContract.Object.IsActive);
            Assert.AreEqual(contractDto.EmployeeId, _mContract.Object.EmployeeId);
            Assert.AreEqual(contractDto.BaseRateOverride, _mContract.Object.BaseRateOverride);
            Assert.AreEqual(contractDto.OvertimeModifierOverride, _mContract.Object.OvertimeModifierOverride);
            Assert.AreEqual(contractDto.ResourceId, _mContract.Object.ResourceId);
            Assert.AreEqual(contractDto.WeeklyHours, _mContract.Object.WeeklyHours);
        }

        [Test]
        public void Should_Map_EmployeeDto_to_Employee()
        {
            // Arrange
            var _mEmployee = new Mock<Employee>();

            var employeeDto = new EmployeeDto
            {
                IsActive = true,
                Address = _testAddressString,
                City = _testCityString,
                Postcode = _testPostcodeString,
                CompanyId = 1,
                EmployeeTypeId = 1,
                GenderId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(employeeDto, _mEmployee.Object);

            // Assert
            Assert.AreEqual(employeeDto.IsActive, _mEmployee.Object.IsActive);
            Assert.AreEqual(employeeDto.Address, _mEmployee.Object.Address);
            Assert.AreEqual(employeeDto.City, _mEmployee.Object.City);
            Assert.AreEqual(employeeDto.Postcode, _mEmployee.Object.Postcode);
            Assert.AreEqual(employeeDto.CompanyId, _mEmployee.Object.CompanyId);
            Assert.AreEqual(employeeDto.EmployeeTypeId, _mEmployee.Object.EmployeeTypeId);
            Assert.AreEqual(employeeDto.GenderId, _mEmployee.Object.GenderId);
        }

        [Test]
        public void Should_Map_EmployeeTypeDto_to_EmployeeType()
        {
            // Arrange
            var _mEmployeeType = new Mock<EmployeeType>();

            var employeeTypeDto = new EmployeeTypeDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(employeeTypeDto, _mEmployeeType.Object);

            // Assert
            Assert.AreEqual(employeeTypeDto.IsActive, _mEmployeeType.Object.IsActive);
            Assert.AreEqual(employeeTypeDto.Name, _mEmployeeType.Object.Name);
        }

        [Test]
        public void Should_Map_GenderDto_to_Gender()
        {
            // Arrange
            var _mGender = new Mock<Gender>();

            var genderDto = new GenderDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService  .MapDtoToEntityForUpdating(genderDto, _mGender.Object);

            // Assert
            Assert.AreEqual(genderDto.IsActive, _mGender.Object.IsActive);
            Assert.AreEqual(genderDto.Name, _mGender.Object.Name);
        }

        [Test]
        public void Should_Map_LeaveReuquestDto_to_LeaveRequest()
        {
            // Arrange
            var _mLeaveRequest = new Mock<LeaveRequest>();

            var leaveRequestDto = new LeaveRequestDto
            {
                IsActive = true,
                EmployeeId = null,
                AmountRequested = 100,
                StartDateTime = _testDateTime,
                EndDateTime = _testDateTime,
                IsApproved = true,
                IsTaken = true,
                LeaveTypeId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(leaveRequestDto, _mLeaveRequest.Object);

            // Assert
            Assert.AreEqual(leaveRequestDto.IsActive, _mLeaveRequest.Object.IsActive);
            Assert.AreEqual(leaveRequestDto.EmployeeId, _mLeaveRequest.Object.EmployeeId);
            Assert.AreEqual(leaveRequestDto.AmountRequested, _mLeaveRequest.Object.AmountRequested);
            Assert.AreEqual(leaveRequestDto.StartDateTime, _mLeaveRequest.Object.StartDateTime);
            Assert.AreEqual(leaveRequestDto.EndDateTime, _mLeaveRequest.Object.EndDateTime);
            Assert.AreEqual(leaveRequestDto.IsApproved, _mLeaveRequest.Object.IsApproved);
            Assert.AreEqual(leaveRequestDto.IsTaken, _mLeaveRequest.Object.IsTaken);
            Assert.AreEqual(leaveRequestDto.LeaveTypeId, _mLeaveRequest.Object.LeaveTypeId);
        }

        [Test]
        public void Should_Map_LeaveTypeDto_to_LeaveType()
        {
            // Arrange
            var _mLeaveType = new Mock<LeaveType>();

            var leaveTypeDto = new LeaveTypeDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(leaveTypeDto, _mLeaveType.Object);

            // Assert
            Assert.AreEqual(leaveTypeDto.IsActive, _mLeaveType.Object.IsActive);
            Assert.AreEqual(leaveTypeDto.Name, _mLeaveType.Object.Name);
        }

        [Test]
        public void Should_map_PersonnelLeaveProfileDto_to_PersonnelLeaveProfile()
        {
            // Arrange
            var _mProfile = new Mock<PersonnelLeaveProfile>();

            var profileDto = new PersonnelLeaveProfileDto
            {
                IsActive = true,
                EmployeeId = 1,
                LeaveTypeId = 1,
                Notes = _testNameString,
                NumberOfDaysAllocated = 20,
                NumberOfDaysRemaining = 10,
                NumberOfDaysTaken = 10
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(profileDto, _mProfile.Object);

            // Assert
            Assert.AreEqual(profileDto.IsActive, _mProfile.Object.IsActive);
            Assert.AreEqual(profileDto.EmployeeId, _mProfile.Object.EmployeeId);
            Assert.AreEqual(profileDto.LeaveTypeId, _mProfile.Object.LeaveTypeId);
            Assert.AreEqual(profileDto.Notes, _mProfile.Object.Notes);
            Assert.AreEqual(profileDto.NumberOfDaysAllocated, _mProfile.Object.NumberOfDaysAllocated);
            Assert.AreEqual(profileDto.NumberOfDaysTaken, _mProfile.Object.NumberOfDaysTaken);
            Assert.AreEqual(profileDto.NumberOfDaysRemaining, _mProfile.Object.NumberOfDaysRemaining);
        }

        [Test]
        public void Should_Map_ResourceDto_to_Resource()
        {
            // Arrange
            var _mResource = new Mock<Resource>();

            var resourceDto = new ResourceDto
            {
                IsActive = true,
                Name = _testNameString,
                BaseRate = 9.75m
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(resourceDto, _mResource.Object);

            // Assert
            Assert.AreEqual(resourceDto.IsActive, _mResource.Object.IsActive);
            Assert.AreEqual(resourceDto.Name, _mResource.Object.Name);
            Assert.AreEqual(resourceDto.BaseRate, _mResource.Object.BaseRate);
        }

        [Test]
        public void Should_Map_ResourceRateModifierDto_to_ResourceRateModifier()
        {
            // Arrange
            var _mModifier = new Mock<ResourceRateModifier>();

            var modifierDto = new ResourceRateModifierDto
            {
                IsActive = true,
                Name = _testNameString,
                Value = 10.9,
                ResourceId = 1,
                ShiftTypeId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(modifierDto, _mModifier.Object);

            // Assert
            Assert.AreEqual(modifierDto.IsActive, _mModifier.Object.IsActive);
            Assert.AreEqual(modifierDto.Name, _mModifier.Object.Name);
            Assert.AreEqual(modifierDto.Value, _mModifier.Object.Value);
            Assert.AreEqual(modifierDto.ResourceId, _mModifier.Object.ResourceId);
            Assert.AreEqual(modifierDto.ShiftTypeId, _mModifier.Object.ShiftTypeId);
        }

        [Test]
        public void Should_Map_ShiftDto_to_Shift()
        {
            // Arrange
            var _mShift = new Mock<Shift>();

            var shiftDto = new ShiftDto
            {
                IsActive = true,
                IsAssigned = true,
                ShiftTemplateId = 1,
                CalendarResourceRequirementId = 1,
                StartDate = new DateTime(),
                EndDate = new DateTime(),
                EmployeeId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(shiftDto, _mShift.Object);

            // Assert
            Assert.AreEqual(shiftDto.IsActive, _mShift.Object.IsActive);
            Assert.AreEqual(shiftDto.IsAssigned, _mShift.Object.IsAssigned);
            Assert.AreEqual(shiftDto.ShiftTemplateId, _mShift.Object.ShiftTemplateId);
            Assert.AreEqual(shiftDto.StartDate, _mShift.Object.StartDate);
            Assert.AreEqual(shiftDto.EndDate, _mShift.Object.EndDate);
            Assert.AreEqual(shiftDto.EmployeeId, _mShift.Object.EmployeeId);
        }

        [Test]
        public void Should_Map_ShiftProfileDto_to_ShiftProfile()
        {
            // Arrange
            var _mShiftProfile = new Mock<ShiftProfile>();

            var shiftProfileDto = new ShiftProfileDto
            {
                IsActive = true,
                StartDateTime = _testDateTime,
                EndDateTime = _testDateTime,
                EmployeeId = 1,
                //HoursWorked = 12,
                ShiftId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(shiftProfileDto, _mShiftProfile.Object);

            // Assert
            Assert.AreEqual(shiftProfileDto.IsActive, _mShiftProfile.Object.IsActive);
            Assert.AreEqual(shiftProfileDto.StartDateTime, _mShiftProfile.Object.StartDateTime);
            Assert.AreEqual(shiftProfileDto.EndDateTime, _mShiftProfile.Object.EndDateTime);
            Assert.AreEqual(shiftProfileDto.HoursWorked, _mShiftProfile.Object.HoursWorked);
            Assert.AreEqual(shiftProfileDto.EmployeeId, _mShiftProfile.Object.EmployeeId);
            Assert.AreEqual(shiftProfileDto.ShiftId, _mShiftProfile.Object.ShiftId);
        }

        [Test]
        public void Should_Map_ShiftTemplateDto_to_ShiftTemplate()
        {
            // Arrange
            var _mshiftTemplate = new Mock<ShiftTemplate>();

            var shiftTemplateDto = new ShiftTemplateDto
            {
                IsActive = true,
                Duration = 8.5,
                EndTime = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                Name = "RMN Day",
                ShiftRate = 9.5m,
                ResourceId = 1,
                SiteId = 13,
                SubSiteId = null,
                UnpaidBreakDuration = 0.5,

                Mon = true,
                Tue = true,
                Wed = true,
                Thu = true,
                Fri = true,
                Sat = false,
                Sun = false
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(shiftTemplateDto, _mshiftTemplate.Object);

            // Assert
            Assert.AreEqual(shiftTemplateDto.IsActive, _mshiftTemplate.Object.IsActive);
            Assert.AreEqual(shiftTemplateDto.Name, _mshiftTemplate.Object.Name);
            Assert.AreEqual(shiftTemplateDto.ShiftTypeId, _mshiftTemplate.Object.ShiftTypeId);
            Assert.AreEqual(shiftTemplateDto.ResourceId, _mshiftTemplate.Object.ResourceId);
            Assert.AreEqual(shiftTemplateDto.SiteId, _mshiftTemplate.Object.SiteId);
            Assert.AreEqual(shiftTemplateDto.SubSiteId, _mshiftTemplate.Object.SubSiteId);
            Assert.AreEqual(shiftTemplateDto.StartTime, _mshiftTemplate.Object.StartTime);
            Assert.AreEqual(shiftTemplateDto.EndTime, _mshiftTemplate.Object.EndTime);
            Assert.AreEqual(shiftTemplateDto.Duration, _mshiftTemplate.Object.Duration);
            Assert.AreEqual(shiftTemplateDto.UnpaidBreakDuration, _mshiftTemplate.Object.UnpaidBreakDuration);
            Assert.AreEqual(shiftTemplateDto.ShiftRate, _mshiftTemplate.Object.ShiftRate);

            Assert.AreEqual(shiftTemplateDto.Mon, _mshiftTemplate.Object.Mon);
            Assert.AreEqual(shiftTemplateDto.Tue, _mshiftTemplate.Object.Tue);
            Assert.AreEqual(shiftTemplateDto.Wed, _mshiftTemplate.Object.Wed);
            Assert.AreEqual(shiftTemplateDto.Thu, _mshiftTemplate.Object.Thu);
            Assert.AreEqual(shiftTemplateDto.Fri, _mshiftTemplate.Object.Fri);
            Assert.AreEqual(shiftTemplateDto.Sat, _mshiftTemplate.Object.Sat);
            Assert.AreEqual(shiftTemplateDto.Sun, _mshiftTemplate.Object.Sun);
        }

        [Test]
        public void Should_Map_ShiftTypeDto_to_ShiftType()
        {
            // Arrange
            var _mShiftType = new Mock<ShiftType>();

            var shiftTypeDto = new ShiftTypeDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(shiftTypeDto, _mShiftType.Object);

            // Assert
            Assert.AreEqual(shiftTypeDto.IsActive, _mShiftType.Object.IsActive);
            Assert.AreEqual(shiftTypeDto.Name, _mShiftType.Object.Name);
        }

        [Test]
        public void Should_Map_SiteDto_to_Site()
        {
            // Arrange
            var _mSite = new Mock<Site>();

            var siteDto = new SiteDto
            {
                IsActive = true,
                Name = _testNameString,
                CompanyId = 1,
                PayrollStartDate = 21,
                SiteTypeId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(siteDto, _mSite.Object);

            // Assert
            Assert.AreEqual(siteDto.IsActive, _mSite.Object.IsActive);
            Assert.AreEqual(siteDto.Name, _mSite.Object.Name);
            Assert.AreEqual(siteDto.CompanyId, _mSite.Object.CompanyId);
            Assert.AreEqual(siteDto.PayrollStartDate, _mSite.Object.PayrollStartDate);
            Assert.AreEqual(siteDto.SiteTypeId, _mSite.Object.SiteTypeId);
        }

        [Test]
        public void Should_Map_SiteTypeDto_to_SiteType()
        {
            // Arrange
            var _mSiteTyep = new Mock<SiteType>();

            var siteTypeDto = new SiteTypeDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(siteTypeDto, _mSiteTyep.Object);

            // Assert
            Assert.AreEqual(siteTypeDto.IsActive, _mSiteTyep.Object.IsActive);
            Assert.AreEqual(siteTypeDto.Name, _mSiteTyep.Object.Name);
        }

        [Test]
        public void SHould_Map_SubSiteDto_to_SubSite()
        {
            // Arrange
            var _mSubSite = new Mock<SubSite>();

            var subSiteDto = new SubSiteDto
            {
                IsActive = true,
                Name = _testNameString,
                SiteId = 1,
                SubSiteTypeId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(subSiteDto, _mSubSite.Object);

            // Assert
            Assert.AreEqual(subSiteDto.IsActive, _mSubSite.Object.IsActive);
            Assert.AreEqual(subSiteDto.Name, _mSubSite.Object.Name);
            Assert.AreEqual(subSiteDto.SiteId, _mSubSite.Object.SiteId);
            Assert.AreEqual(subSiteDto.SubSiteTypeId, _mSubSite.Object.SubSiteTypeId);
        }

        [Test]
        public void Should_Map_SubSiteTypeDto_to_SubSiteType()
        {
            // Arrange
            var _mSubSiteType = new Mock<SubSiteType>();

            var subSiteTypeDto = new SubSiteTypeDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(subSiteTypeDto, _mSubSiteType.Object);

            // Assert
            Assert.AreEqual(subSiteTypeDto.IsActive, _mSubSiteType.Object.IsActive);
            Assert.AreEqual(subSiteTypeDto.Name, _mSubSiteType.Object.Name);
        }

        [Test]
        public void Should_Map_SitePersonnelLookupDto_to_SitePersonnelLookup()
        {
            // Arrange
            var _mSitePersonnelLookup = new Mock<SitePersonnelLookup>();

            var lookupDto = new SitePersonnelLookupDto
            {
                IsActive = true,
                EmployeeId = 1,
                SiteId = 1,
                SubSiteId = null
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(lookupDto, _mSitePersonnelLookup.Object);

            // Assert
            Assert.AreEqual(lookupDto.IsActive, _mSitePersonnelLookup.Object.IsActive);
            Assert.AreEqual(lookupDto.EmployeeId, _mSitePersonnelLookup.Object.EmployeeId);
            Assert.AreEqual(lookupDto.SiteId, _mSitePersonnelLookup.Object.SiteId);
            Assert.AreEqual(lookupDto.SubSiteId, _mSitePersonnelLookup.Object.SubSiteId);
        }

        [Test]
        public void Should_Map_SystemAccessRoleDto_to_SystemAccessRole()
        {
            // Arrange
            var _mRole = new Mock<SystemAccessRole>();

            var roleDto = new SystemAccessRoleDto
            {
                IsActive = true,
                Name = _testNameString
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(roleDto, _mRole.Object);

            // Assert
            Assert.AreEqual(roleDto.IsActive, _mRole.Object.IsActive);
            Assert.AreEqual(roleDto.Name, _mRole.Object.Name);
        }

        [Test]
        public void Should_Map_UserDto_to_User()
        {
            // Arrange
            var _mUser = new Mock<User>();

            var userDto = new UserDto
            {
                IsActive = true,
                Email = _testEmailString,
                Firstname = _testNameString,
                Lastname = _testNameString,
                Password = _testNameString,
                IsAccountLocked = false,
                Login = _testNameString,
                SystemAccessRoleId = 1
            };

            // Act
            CommonHelperAppService.MapDtoToEntityForUpdating(userDto, _mUser.Object);

            // Assert
            Assert.AreEqual(userDto.IsActive, _mUser.Object.IsActive);
            Assert.AreEqual(userDto.Email, _mUser.Object.Email);
            Assert.AreEqual(userDto.Firstname, _mUser.Object.Firstname);
            Assert.AreEqual(userDto.Lastname, _mUser.Object.Lastname);
            Assert.AreEqual(userDto.Password, _mUser.Object.Password);
            Assert.AreEqual(userDto.IsAccountLocked, _mUser.Object.IsAccountLocked);
            Assert.AreEqual(userDto.Login, _mUser.Object.Login);
            Assert.AreEqual(userDto.SystemAccessRoleId, _mUser.Object.SystemAccessRoleId);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
