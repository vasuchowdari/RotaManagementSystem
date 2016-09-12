using System;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;

namespace RMS.AppServiceLayer.Mailers.Interfaces
{
    public interface IMailerAppService : IDisposable
    {
        // Service Methods
        void SendTimeAdjustmentForm(TimeAdjustmentFormDto timeAdjustmentFormDto);
        void SendLeaveRequestEmail(LeaveRequestDto leaveRequestDto);
    }
}
