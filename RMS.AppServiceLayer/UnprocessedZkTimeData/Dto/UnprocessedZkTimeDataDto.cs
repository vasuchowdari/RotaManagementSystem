using System;
using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.UnprocessedZkTimeData.Dto
{
    public class UnprocessedZkTimeDataDto : BaseDto
    {
        public DateTime ClockingTime { get; set; }

        // zk data
        public int ZkTimeUserId { get; set; }
        public int ZkTimeBadgeNumber { get; set; }
        public string ZkTimeUserName { get; set; }

        public int ZkTimeSiteNumber { get; set; }
        public string ZkTimeSiteName { get; set; }

        // rms data
        //public long RmsUserId { get; set; }
        //public long RmsSiteId { get; set; }

        // may not have a shift id to assign
        public long? ShiftId { get; set; }
        //public TimeSpan? Overtime { get; set; }
    }
}
