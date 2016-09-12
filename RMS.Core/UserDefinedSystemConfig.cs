namespace RMS.Core
{
    public class UserDefinedSystemConfig : BaseEntity
    {
        public long CompanyId { get; set; }

        // ZKTime System threshold values
        public double NHoursAgo { get; set; }
        public double ShiftPostStartStillValidThresholdValue { get; set; }
        public double ShiftPreStartEarlyInThresholdValue { get; set; }
        public double ShiftPostEndValidThresholdValue { get; set; }

        public int PayrollStartDayOfMonth { get; set; }
        public int PayrollEndDayOfMonth { get; set; }
        public double NICFactor { get; set; }
        public double AccruedHolidayFactor { get; set; }
    }
}
