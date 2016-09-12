using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Employees.Dto;
using RMS.AppServiceLayer.Employees.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.Shifts.Dto;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Employees.Services
{
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public EmployeeAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }


        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public StaffCalendarDto GetPersonalCalendarObjectsByDateRange(long employeeId, DateTime startDate,
            DateTime endDate)
        {
            var shifts = _unitOfWork.ShiftRepository.Get(s => s.IsActive &&
                                                              s.EmployeeId == employeeId &&
                                                              s.StartDate >= startDate &&
                                                              s.EndDate <= endDate)
                                                    .ToList();

            var leave = _unitOfWork.LeaveRequestRepository.Get(l => l.IsActive &&
                                                                    l.EmployeeId == employeeId &&
                                                                    l.StartDateTime >= startDate &&
                                                                    l.EndDateTime <= endDate)
                                                          .ToList();

            var staffCalendarDto = new StaffCalendarDto();
            staffCalendarDto.ShiftDtos = new List<ShiftDto>();

            if (shifts != null)
            {
                foreach (var shift in shifts)
                {
                    var shiftDto = Mapper.Map<ShiftDto>(shift);
                    staffCalendarDto.ShiftDtos.Add(shiftDto);
                }
            }

            staffCalendarDto.LeaveRequestDtos = new List<LeaveRequestDto>();
            if (leave != null)
            {
                foreach (var leaveRequest in leave)
                {
                    var leaveRequestDto = Mapper.Map<LeaveRequestDto>(leaveRequest);
                    staffCalendarDto.LeaveRequestDtos.Add(leaveRequestDto);
                }
            }

            return staffCalendarDto;
        }

        // Repo Methods
        public EmployeeDto GetById(long id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(e => e.Id == id && e.IsActive,
                null,
                "User, Contracts")
                .FirstOrDefault();

            if (employee != null)
            {
                var employeeDto = Mapper.Map<EmployeeDto>(employee);

                return employeeDto;    
            }

            return null;
        }

        public ICollection<EmployeeDto> GetForCompany(long companyId)
        {
            var employees = _unitOfWork.EmployeeRepository
                .Get(e => e.CompanyId == companyId && e.IsActive,
                null, 
                "User, Contracts, LeaveRequests, LeaveProfiles");

            if (employees != null)
            {
                var employeeDtos = new List<EmployeeDto>();

                foreach (var employee in employees)
                {
                    employeeDtos.Add(Mapper.Map<EmployeeDto>(employee));
                }

                return employeeDtos;
            }

            return null;
        }

        public ICollection<EmployeeDto> GetAllForCompany(long companyId)
        {
            var employees = _unitOfWork.EmployeeRepository
                .Get(e => e.CompanyId == companyId,
                null,
                "User, Contracts, LeaveRequests, LeaveProfiles");

            if (employees != null)
            {
                var employeeDtos = new List<EmployeeDto>();

                foreach (var employee in employees)
                {
                    employeeDtos.Add(Mapper.Map<EmployeeDto>(employee));
                }

                return employeeDtos;
            }

            return null;
        }

        public ICollection<EmployeeDto> GetAll()
        {
            var employees = _unitOfWork.EmployeeRepository.Get(e => e.IsActive, null, "User").ToList();

            if (employees != null)
            {
                var employeeDtos = new List<EmployeeDto>();

                foreach (var employee in employees)
                {
                    employeeDtos.Add(Mapper.Map<EmployeeDto>(employee));
                }

                return employeeDtos;
            }

            return null;
        }

        public EmployeeDto GetByUserId(long userId)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(e => e.UserId == userId,
                null,
                "Contracts, User")
                .FirstOrDefault();

            if (employee != null)
            {
                var employeeDto = Mapper.Map<EmployeeDto>(employee);

                return employeeDto;
            }

            return null;
        }

        public ICollection<EmployeeNameIdDto> GetEmployeeNameAndId()
        {
            var employees = _unitOfWork.EmployeeRepository.GetAll();
            var employeeNameIdList = new List<EmployeeNameIdDto>();

            foreach (var employee in employees)
            {
                //var employeeUserId = employee.UserId;
                var employeeNameIdItem = new EmployeeNameIdDto();
                employeeNameIdItem.Id = employee.Id;

                var employeeUserObj = _unitOfWork.UserRepository.Get(u => u.Id == employee.UserId)
                                                                .FirstOrDefault();

                employeeNameIdItem.Firstname = employeeUserObj.Firstname;
                employeeNameIdItem.Lastname = employeeUserObj.Lastname;

                employeeNameIdList.Add(employeeNameIdItem);
            }

            return employeeNameIdList;
        }


        // CRUD
        public long Create(EmployeeDto employeeDto, long userId)
        {
            var employee = Mapper.Map<Employee>(employeeDto);

            _unitOfWork.EmployeeRepository.Create(employee);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.EmployeeTableName,
                userId,
                employee.Id);

            return employee.Id;
        }

        public void Update(EmployeeDto employeeDto, long userId)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(employeeDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(employeeDto, employee);

            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.EmployeeTableName,
                userId,
                employee.Id);
        }

        public void Delete(long id, long userId)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);

            employee.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.EmployeeTableName,
                userId,
                employee.Id);
        }
    }
}
