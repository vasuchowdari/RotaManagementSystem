using RMS.AppServiceLayer.Helpers.Services;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.Mailers.Interfaces;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;

namespace RMS.AppServiceLayer.Mailers.Services
{
    public class MailerAppService : IMailerAppService
    {
        public void SendTimeAdjustmentForm(TimeAdjustmentFormDto timeAdjustmentFormDto)
        {
            MailerService.SendTimeAdjustmentEmail(timeAdjustmentFormDto);
        }

        public void SendLeaveRequestEmail(LeaveRequestDto leaveRequestDto)
        {
            throw new System.NotImplementedException();
//            MailerService.SendLeaveRequestEmail(leaveRequestDto);
        }


        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }
    }
}
