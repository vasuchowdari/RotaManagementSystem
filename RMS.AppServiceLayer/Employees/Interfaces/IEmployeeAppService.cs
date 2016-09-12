using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Employees.Dto;

namespace RMS.AppServiceLayer.Employees.Interfaces
{
    public interface IEmployeeAppService : IDisposable
    {
        // Service Methods
        StaffCalendarDto GetPersonalCalendarObjectsByDateRange(long employeeId, DateTime startDate,
            DateTime endDate);

        // Repo Methods
        EmployeeDto GetById(long id);
        EmployeeDto GetByUserId(long userId);
        ICollection<EmployeeDto> GetForCompany(long companyId);
        ICollection<EmployeeDto> GetAll();
        ICollection<EmployeeDto> GetAllForCompany(long companyId);
        ICollection<EmployeeNameIdDto> GetEmployeeNameAndId();

        // CRUD
        long Create(EmployeeDto employeeDto, long userId);
        void Update(EmployeeDto employeeDto, long userId);
        void Delete(long id, long userId);
    }
}
