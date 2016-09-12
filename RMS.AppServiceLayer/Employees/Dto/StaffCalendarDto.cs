using System.Collections.Generic;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.Shifts.Dto;

namespace RMS.AppServiceLayer.Employees.Dto
{
    public class StaffCalendarDto
    {
        public ICollection<ShiftDto> ShiftDtos { get; set; }
        public ICollection<LeaveRequestDto> LeaveRequestDtos { get; set; } 
    }
}
