using System;

namespace RMS.Core
{
    public class ZkTimeClockingRecord : BaseEntity
    {
        public DateTime ClockingTime { get; set; }

        // zk time data
        public int ZkTimeUserId { get; set; } 
        public int ZkTimeBadgeNumber { get; set; }
        public string ZkTimeUserName { get; set; }
        public int ZkTimeSiteNumber { get; set; }
        public string ZkTimeSiteName { get; set; }

        public long? ShiftId { get; set; }
        public long? TshiftId { get; set; }

        public bool IsMatched { get; set; }
    }
}
