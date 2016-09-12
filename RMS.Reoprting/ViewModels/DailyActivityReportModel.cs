namespace RMS.Reoprting.ViewModels
{
    public class DailyActivityReportModel
    {
        public string Period { get; set; }
        public string SiteLocation { get; set; }
        public string SubSiteLocation { get; set; }
        public string Role { get; set; }
        public int ZKTimeSystemId { get; set; }
        public string PayrollReferenceNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string RmsShiftStart { get; set; }
        public string RmsShiftEnd { get; set; }
        public double ShiftHours { get; set; }
        public int UnpaidBreak { get; set; }
        public string ZKTimeClockIn { get; set; }
        public string ZKTimeClockOut { get; set; }
        public string ActualStart { get; set; }
        public string ActualEnd { get; set; }
        public decimal ActualHours { get; set; }
        public string ActualStatus { get; set; }
        public string Approval { get; set; }
        public string Modified { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
    }
}
