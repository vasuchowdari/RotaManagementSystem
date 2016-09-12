using System;

namespace RMS.WebAPI.Models
{
    public class ShiftProfileModel : BaseModel
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

        public ShiftModel ShiftModel { get; set; }
    }
}