using System;
using System.Collections.Generic;

namespace RMS.Core
{
    public class CalendarResourceRequirement : BaseEntity
    {
        public long CalendarId { get; set; }
        public long ResourceId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Calendar Calendar { get; set; }
        public Resource Resource { get; set; }

        public ICollection<Shift> Shifts { get; set; }
    }
}
