using System;

namespace RMS.WebAPI.ViewModels
{
    public class ZktUnmatchedByEmployeeModel
    {
        public string EmployeeName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}