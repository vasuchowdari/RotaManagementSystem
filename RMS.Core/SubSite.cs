using System.Collections.Generic;
using System.Diagnostics;

namespace RMS.Core
{
    public class SubSite : BaseEntity
    {
        public string Name { get; set; }
        public long SubSiteTypeId { get; set; }
        public long SiteId { get; set; }

        public ICollection<Calendar> Calendars { get; set; } 
    }
}
