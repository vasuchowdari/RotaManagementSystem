using System;

namespace RMS.WebAPI.ViewModels
{
    public class TimeAdjustmentFormAndShiftProfileUpdateModel
    {
        public long TimeAdjustmentFormId { get; set; }
        public long ShiftProfileId { get; set; }
        public bool IsManagerApproved { get; set; }
        public bool IsAdminApproved { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }

        public DateTime ActualStartDateTime { get; set; }
        public DateTime ActualEndDateTime { get; set; }

        public DateTime ZktStartDateTime { get; set; }
        public DateTime ZktEndDateTime { get; set; }
    }
}