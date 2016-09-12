using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.ShiftProfiles.Dto;

namespace RMS.AppServiceLayer.ShiftProfiles.Interfaces
{
    public interface IShiftProfileAppService : IDisposable
    {
        // Service Methods
        int ConsecutiveDayCalculatorDefault(long id, bool isEmployee);
        int ConsecutiveDayCalculator(long id, bool isEmployee, DateTime startDate, DateTime endDate);
        ICollection<ShiftProfileDto> ReturnOrderedShiftProfiles();
        //List<List<ShiftProfileDto>> GetInvalidShiftProfiles();
        List<ShiftProfileDto> GetInvalidShiftProfiles();
        List<ShiftProfileDto> GetInvalidForEmployee(long employeeId);
        ICollection<ShiftProfileDto> GetAllForShift(long id);
        ShiftProfileDto CheckApproval(long id);    

        // Repo Methods
        ShiftProfileDto GetById(long id);
        ICollection<ShiftProfileDto> GetAll();

        // CRUD
        void Create(ShiftProfileDto shiftProfileDto, long userId);
        void Update(ShiftProfileDto shiftProfileDto, long userId);
        void Delete(long id, long userId);
    }
}
