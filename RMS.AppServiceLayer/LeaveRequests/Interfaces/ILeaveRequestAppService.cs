using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.LeaveRequests.Dto;

namespace RMS.AppServiceLayer.LeaveRequests.Interfaces
{
    public interface ILeaveRequestAppService : IDisposable
    {
        // Service Methods
        ICollection<LeaveRequestDto> GetForEmployee(long id);
        ICollection<LeaveRequestDto> GetForIds(List<long> staffIds);

        // Repo Methods
        LeaveRequestDto GetById(long id);
        ICollection<LeaveRequestDto> GetAll();

        // CRUD
        int Create(LeaveRequestDto leaveRequestDto, long userId);
        int Update(LeaveRequestDto leaveRequestDto, long userId);
        void Delete(long id, long userId);
    }
}
