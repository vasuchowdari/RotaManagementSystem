using System;

namespace RMS.Zktime.Dtos
{
    [Serializable]
    public class ZkTimeDataToImportModel
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public DateTime ClockingTime { get; set; }
        public string ClockingType { get; set; }

        public long? ShiftId { get; set; }
        public TimeSpan Overtime { get; set; }
    }
}
