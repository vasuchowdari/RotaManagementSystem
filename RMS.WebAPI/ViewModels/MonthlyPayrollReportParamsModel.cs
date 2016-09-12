using System;

namespace RMS.WebAPI.ViewModels
{
    public class MonthlyPayrollReportParamsModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}