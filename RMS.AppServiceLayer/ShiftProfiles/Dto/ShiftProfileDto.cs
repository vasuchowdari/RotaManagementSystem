using System;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Shifts.Dto;

namespace RMS.AppServiceLayer.ShiftProfiles.Dto
{
    public class ShiftProfileDto : BaseDto
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public DateTime ActualStartDateTime { get; set; }
        public DateTime ActualEndDateTime { get; set; }

        public DateTime ZktStartDateTime { get; set; }
        public DateTime ZktEndDateTime { get; set; }

        public long HoursWorked { get; set; }
        public TimeSpan TimeWorked { get; set; }
        
        public long? EmployeeId { get; set; }
        public long ShiftId { get; set; }
        public bool IsApproved { get; set; }
        public int Status { get; set; }

        public string Reason { get; set; }
        public string Notes { get; set; }
        public bool IsModified { get; set; }

        public ShiftDto ShiftDto { get; set; }
    }
}
