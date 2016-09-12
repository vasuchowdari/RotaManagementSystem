using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Shifts.Dto;

namespace RMS.AppServiceLayer.Shifts.Interfaces
{
    public interface IShiftAppService : IDisposable
    {
        // Service Methods
        ICollection<long> CheckIfStaffAssignedToShiftByDateRange(ICollection<long> staffIds, DateTime shiftRangeStartDate, DateTime shiftRangeEndDate);
        string GetSiteName(long shiftId);
        bool CheckIfShiftExistsForResourceReq(ShiftDto shiftDto);


        // Repo Methods
        ShiftDto GetById(long id);
        ICollection<ShiftDto> GetAll();
        ICollection<ShiftDto> GetByEmployeeId(long id);
        ICollection<ShiftDto> GetForCalendarResourceRequirment(long calResRqId, DateTime startDate, DateTime endDate); 

        // CRUD
        void Create(ShiftDto shiftDto, long userId);
        void Update(ShiftDto shiftDto, long userId);
        void Delete(long id, long userId);
    }
}
