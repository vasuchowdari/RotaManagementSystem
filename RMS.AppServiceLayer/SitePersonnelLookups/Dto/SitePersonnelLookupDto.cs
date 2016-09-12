using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.SitePersonnelLookups.Dto
{
    public class SitePersonnelLookupDto : BaseDto
    {
        public long? EmployeeId { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }
    }
}
