using System.Collections.Generic;

namespace RMS.Core
{
    public class Site : BaseEntity
    {
        public string Name { get; set; }
        public int PayrollStartDate { get; set; }
        public int PayrollEndDate { get; set; }
        public long SiteTypeId { get; set; }
        public long CompanyId { get; set; }

        public SiteType SiteType { get; set; }
        public Company Company { get; set; }

        public ICollection<SubSite> SubSites { get; set; }
        public ICollection<Calendar> Calendars { get; set; }
    }
}
