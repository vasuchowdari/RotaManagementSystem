using System;
using System.Data.Linq.Mapping;

namespace RMS.Zktime.Classes
{
    [Table(Name="CHECKINOUT")]
    public class Clocking
    {
        [Column(Name="USERID")]
        public int UserId { get; set; }

        [Column(Name="CHECKTIME")]
        public DateTime ClockingTime { get; set; }

        [Column(Name="CHECKTYPE")]
        public string ClockingType { get; set; }

        [Column(Name="SENSORID")]
        public int MachineNumber { get; set; }
    }
}
