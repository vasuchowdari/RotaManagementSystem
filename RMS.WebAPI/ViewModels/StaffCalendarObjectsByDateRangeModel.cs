using System;

namespace RMS.WebAPI.ViewModels
{
    public class StaffCalendarObjectsByDateRangeModel
    {
        public long EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}