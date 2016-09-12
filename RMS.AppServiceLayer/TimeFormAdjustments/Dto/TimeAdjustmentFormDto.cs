using System;
using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.TimeFormAdjustments.Dto
{
    public class TimeAdjustmentFormDto : BaseDto
    {
        public long ShiftId { get; set; }
        public long ShiftProfileId { get; set; }
        public long? EmployeeId { get; set; }
        public string StaffName { get; set; }
        public string ShiftLocation { get; set; }

        public DateTime ShiftStartDateTime { get; set; }
        public DateTime ShiftEndDateTime { get; set; }
        public DateTime ActualStartDateTime { get; set; }
        public DateTime ActualEndDateTime { get; set; }
        public DateTime ZktStartDateTime { get; set; }
        public DateTime ZktEndDateTime { get; set; }

        public bool MissedClockIn { get; set; }
        public bool MissedClockOut { get; set; }
        public bool LateIn { get; set; }
        public bool EarlyOut { get; set; }

        public bool IsManagerApproved { get; set; }
        public bool IsAdminApproved { get; set; }

        public string Notes { get; set; }
    }
}
