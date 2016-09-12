namespace RMS.WebAPI.Models
{
    public class SitePersonnelLookupModel : BaseModel
    {
        public long? EmployeeId { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }
    }
}