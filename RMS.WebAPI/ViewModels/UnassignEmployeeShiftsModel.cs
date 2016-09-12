using System;

namespace RMS.WebAPI.ViewModels
{
    public class UnassignEmployeeShiftsModel
    {
        public long EmployeeId { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}