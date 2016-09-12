namespace RMS.Core
{
    public class SitePersonnelLookup : BaseEntity
    {
        public long? EmployeeId { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }
    }
}
