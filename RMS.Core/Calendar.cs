using System;
using System.Collections.Generic;

namespace RMS.Core
{
    public class Calendar : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }

        public ICollection<CalendarResourceRequirement> CalendarResourceRequirements { get; set; }
    }
}
