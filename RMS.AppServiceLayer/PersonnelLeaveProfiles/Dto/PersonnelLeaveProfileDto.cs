using System;
using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.PersonnelLeaveProfiles.Dto
{
    public class PersonnelLeaveProfileDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double NumberOfDaysTaken { get; set; }
        public double NumberOfDaysAllocated { get; set; }
        public double NumberOfDaysRemaining { get; set; }

        public double NumberOfHoursTaken { get; set; }
        public double NumberOfHoursAllocated { get; set; }
        public double NumberOfHoursRemaining { get; set; }

        public long? EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public string Notes { get; set; }
    }
}
