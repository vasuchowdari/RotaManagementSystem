using System;
using System.Collections.Generic;

namespace RMS.WebAPI.ViewModels
{
    public class CheckIfStaffAssignedToShiftDataModel
    {
        public ICollection<long> EmployeeIds { get; set; }
        public DateTime ShiftRangeStartDate { get; set; }
        public DateTime ShiftRangeEndDate { get; set; }
    }
}