using System;

namespace RMS.WebAPI.ViewModels
{
    public class DailyActivityReportParamsModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SiteName { get; set; }
        public string SubSiteName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RoleName { get; set; }
    }
}