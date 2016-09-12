using RMS.AppServiceLayer.Helpers.Dto;

namespace RMS.AppServiceLayer.Mailers.Dto
{
    public class LeaveRequestEmailDto : BaseEmailDto
    {
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string AmountRequested { get; set; }
        public string StaffName { get; set; }
        public string LeaveTypName { get; set; }
        public string Notes { get; set; }
    }
}
