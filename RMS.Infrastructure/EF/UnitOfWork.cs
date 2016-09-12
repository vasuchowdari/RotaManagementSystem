using System;
using RMS.Core;
using RMS.Infrastructure.EF.Repositories;

namespace RMS.Infrastructure.EF
{
    public class UnitOfWork : IDisposable
    {
        private RmsContext _ctx = new RmsContext();

        private GenericRepository<AuditLog> _auditLogRepository;

        private GenericRepository<Budget> _budgetRepository;
        private GenericRepository<BudgetPeriod> _budgetPeriodRepository;
        private GenericRepository<Calendar> _calendarRepository;
        private GenericRepository<CalendarResourceRequirement> _calendarResourceRequirementRepository;
        private GenericRepository<Company> _companyRepository;
        private GenericRepository<Contract> _contractRepository; 
        private GenericRepository<Employee> _employeeRepository;
        private GenericRepository<EmployeeType> _employeeTypeRepository;
        private GenericRepository<Gender> _genderRepository;
        private GenericRepository<LeaveRequest> _leaveRequestRepository; 
        private GenericRepository<LeaveType> _leaveTypeRepository;
        private GenericRepository<PersonnelLeaveProfile> _personnelLeaveProfileRepository;
        private GenericRepository<Resource> _resourceRepository;
        private GenericRepository<ResourceRateModifier> _resourceRateModifierRepository;
        private GenericRepository<Shift> _shiftRepository;
        private GenericRepository<ShiftProfile> _shiftProfileRepository; 
        private GenericRepository<ShiftType> _shiftTypeRepository;
        private GenericRepository<ShiftTemplate> _shiftTemplateRepository; 
        private GenericRepository<Site> _siteRepository;
        private GenericRepository<SiteType> _siteTypeRepository;
        private GenericRepository<SubSite> _subSiteRepository; 
        private GenericRepository<SubSiteType> _subSiteTypeRepository;
        private GenericRepository<SitePersonnelLookup> _sitePersonnelLookupRepository;
        private GenericRepository<SystemAccessRole> _systemAccessRoleRepository;
        private GenericRepository<TimeAdjustmentForm> _timeAdjustmentFormRepository;
        private GenericRepository<UserDefinedSystemConfig> _userDefinedSystemConfigRepository; 
        private GenericRepository<User> _userRepository;
        private GenericRepository<ZkTimeClockingRecord> _zktimeClockingRecordRepository; 

        // Repo Declarations
        public GenericRepository<AuditLog> AuditLogRepository
        {
            get
            {
                if (_auditLogRepository == null)
                {
                    _auditLogRepository = new GenericRepository<AuditLog>(_ctx);
                }

                return _auditLogRepository;
            }
        }

        public GenericRepository<Budget> BudgetRepository
        {
            get
            {
                if (_budgetRepository == null)
                {
                    _budgetRepository = new GenericRepository<Budget>(_ctx);
                }

                return _budgetRepository;
            }
        }

        public GenericRepository<BudgetPeriod> BudgetPeriodRepository
        {
            get
            {
                if (_budgetPeriodRepository == null)
                {
                    _budgetPeriodRepository = new GenericRepository<BudgetPeriod>(_ctx);
                }

                return _budgetPeriodRepository;
            }
        }

        public GenericRepository<Calendar> CalendarRepository
        {
            get
            {
                if (_calendarRepository == null)
                {
                    _calendarRepository = new GenericRepository<Calendar>(_ctx);
                }

                return _calendarRepository;
            }
        }

        public GenericRepository<CalendarResourceRequirement> CalendarResourceRequirementRepository
        {
            get
            {
                if (_calendarResourceRequirementRepository == null)
                {
                    _calendarResourceRequirementRepository = new GenericRepository<CalendarResourceRequirement>(_ctx);
                }

                return _calendarResourceRequirementRepository;
            }
        }

        public GenericRepository<Company> CompanyRepository
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new GenericRepository<Company>(_ctx);
                }

                return _companyRepository;
            }
        }

        public GenericRepository<Contract> ContractRepository
        {
            get
            {
                if (_contractRepository == null)
                {
                    _contractRepository = new GenericRepository<Contract>(_ctx);
                }

                return _contractRepository;
            }
        } 

        public GenericRepository<Employee> EmployeeRepository
        {
            get
            {
                if (_employeeRepository == null)
                {
                    _employeeRepository = new GenericRepository<Employee>(_ctx);
                }

                return _employeeRepository;
            }
        }

        public GenericRepository<EmployeeType> EmployeeTypeRepository
        {
            get
            {
                if (_employeeTypeRepository == null)
                {
                    _employeeTypeRepository = new GenericRepository<EmployeeType>(_ctx);
                }

                return _employeeTypeRepository;
            }
        }

        public GenericRepository<Gender> GenderRepository
        {
            get
            {
                if (_genderRepository == null)
                {
                    _genderRepository = new GenericRepository<Gender>(_ctx);
                }

                return _genderRepository;
            }
        }

        public GenericRepository<LeaveRequest> LeaveRequestRepository
        {
            get
            {
                if (_leaveRequestRepository == null)
                {
                    _leaveRequestRepository = new GenericRepository<LeaveRequest>(_ctx);
                }

                return _leaveRequestRepository;
            }
        } 

        public GenericRepository<LeaveType> LeaveTypeRepository
        {
            get
            {
                if (_leaveTypeRepository == null)
                {
                    _leaveTypeRepository = new GenericRepository<LeaveType>(_ctx);
                }

                return _leaveTypeRepository;
            }
        }

        public GenericRepository<PersonnelLeaveProfile> PersonnelLeaveProfileRepository
        {
            get
            {
                if (_personnelLeaveProfileRepository == null)
                {
                    _personnelLeaveProfileRepository = new GenericRepository<PersonnelLeaveProfile>(_ctx);
                }

                return _personnelLeaveProfileRepository;
            }
        }

        public GenericRepository<ResourceRateModifier> ResourceRateModifierRepository
        {
            get
            {
                if (_resourceRateModifierRepository == null)
                {
                    _resourceRateModifierRepository = new GenericRepository<ResourceRateModifier>(_ctx);
                }

                return _resourceRateModifierRepository;
            }
        } 

        public GenericRepository<Resource> ResourceRepository
        {
            get
            {
                if (_resourceRepository == null)
                {
                    _resourceRepository = new GenericRepository<Resource>(_ctx);
                }

                return _resourceRepository;
            }
        }

        public GenericRepository<Shift> ShiftRepository
        {
            get
            {
                if (_shiftRepository == null)
                {
                    _shiftRepository = new GenericRepository<Shift>(_ctx);
                }

                return _shiftRepository;
            }
        }

        public GenericRepository<ShiftProfile> ShiftProfileRepository
        {
            get
            {
                if (_shiftProfileRepository == null)
                {
                    _shiftProfileRepository = new GenericRepository<ShiftProfile>(_ctx);
                }

                return _shiftProfileRepository;
            }
        }

        public GenericRepository<ShiftTemplate> ShiftTemplateRepository
        {
            get
            {
                if (_shiftTemplateRepository == null)
                {
                    _shiftTemplateRepository = new GenericRepository<ShiftTemplate>(_ctx);
                }

                return _shiftTemplateRepository;
            }
        }

        public GenericRepository<ShiftType> ShiftTypeRepository
        {
            get
            {
                if (_shiftTypeRepository == null)
                {
                    _shiftTypeRepository = new GenericRepository<ShiftType>(_ctx);
                }

                return _shiftTypeRepository;
            }
        }

        public GenericRepository<Site> SiteRepository
        {
            get
            {
                if (_siteRepository == null)
                {
                    _siteRepository = new GenericRepository<Site>(_ctx);
                }

                return _siteRepository;
            }
        }

        public GenericRepository<SiteType> SiteTypeRepository
        {
            get
            {
                if (_siteTypeRepository == null)
                {
                    _siteTypeRepository = new GenericRepository<SiteType>(_ctx);
                }

                return _siteTypeRepository;
            }
        }

        public GenericRepository<SubSite> SubSiteRepository
        {
            get
            {
                if (_subSiteRepository == null)
                {
                    _subSiteRepository = new GenericRepository<SubSite>(_ctx);
                }

                return _subSiteRepository;
            }
        }

        public GenericRepository<SubSiteType> SubSiteTypeRepository
        {
            get
            {
                if (_subSiteTypeRepository == null)
                {
                    _subSiteTypeRepository = new GenericRepository<SubSiteType>(_ctx);
                }

                return _subSiteTypeRepository;
            }
        }

        public GenericRepository<SitePersonnelLookup> SitePersonnelLookupRepository
        {
            get
            {
                if (_sitePersonnelLookupRepository == null)
                {
                    _sitePersonnelLookupRepository = new GenericRepository<SitePersonnelLookup>(_ctx);
                }

                return _sitePersonnelLookupRepository;
            }
        }

        public GenericRepository<SystemAccessRole> SystemAccessRoleRepository
        {
            get
            {
                if (_systemAccessRoleRepository == null)
                {
                    _systemAccessRoleRepository= new GenericRepository<SystemAccessRole>(_ctx);
                }

                return _systemAccessRoleRepository;
            }
        }

        public GenericRepository<TimeAdjustmentForm> TimeAdjustmentFormRepository
        {
            get
            {
                if (_timeAdjustmentFormRepository == null)
                {
                    _timeAdjustmentFormRepository = new GenericRepository<TimeAdjustmentForm>(_ctx);
                }

                return _timeAdjustmentFormRepository;
            }
        }

        public GenericRepository<UserDefinedSystemConfig> UserDefinedSystemConfigRepository
        {
            get
            {
                if (_userDefinedSystemConfigRepository == null)
                {
                    _userDefinedSystemConfigRepository = new GenericRepository<UserDefinedSystemConfig>(_ctx);
                }

                return _userDefinedSystemConfigRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_ctx);
                }

                return _userRepository;
            }
        }

        public GenericRepository<ZkTimeClockingRecord> ZkTimeClockingRecordRepository
        {
            get
            {
                if (_zktimeClockingRecordRepository == null)
                {
                    _zktimeClockingRecordRepository = new GenericRepository<ZkTimeClockingRecord>(_ctx);
                }

                return _zktimeClockingRecordRepository;
            }
        }


        public void Save()
        {
            _ctx.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
