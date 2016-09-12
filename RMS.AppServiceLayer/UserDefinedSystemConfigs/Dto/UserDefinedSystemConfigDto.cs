﻿using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto
{
    public class UserDefinedSystemConfigDto : BaseDto
    {
        public long CompanyId { get; set; }

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
