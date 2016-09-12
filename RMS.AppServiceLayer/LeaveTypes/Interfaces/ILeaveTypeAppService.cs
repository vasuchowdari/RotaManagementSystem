using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.LeaveTypes.Dto;

namespace RMS.AppServiceLayer.LeaveTypes.Interfaces
{
    public interface ILeaveTypeAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        LeaveTypeDto GetById(long id);
        ICollection<LeaveTypeDto> GetAll();

        // CRUD
        void Create(LeaveTypeDto leaveTypeDto, long userId);
        void Update(LeaveTypeDto leaveTypeDto, long userId);
        void Delete(long id, long userId);
    }
}
