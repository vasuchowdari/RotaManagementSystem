using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.EmployeeTypes.Dto;

namespace RMS.AppServiceLayer.EmployeeTypes.Interfaces
{
    public interface IEmployeeTypeAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        EmployeeTypeDto GetById(long id);
        ICollection<EmployeeTypeDto> GetAll();

        // CRUD
        void Create(EmployeeTypeDto employeeTypeDto, long userId);
        void Update(EmployeeTypeDto employeeTypeDto, long userId);
        void Delete(long id, long userId);
    }
}
