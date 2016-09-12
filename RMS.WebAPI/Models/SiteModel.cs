using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class SiteModel : BaseModel
    {
        public string Name { get; set; }
        public int PayrollStartDate { get; set; }
        public int PayrollEndDate { get; set; }
        public long SiteTypeId { get; set; }
        public long CompanyId { get; set; }

        public SiteTypeModel SiteTypeModel { get; set; }
        public CompanyModel CompanyModel { get; set; }

        public ICollection<SubSiteModel> SubSiteModels { get; set; }
        public ICollection<CalendarModel> CalendarModels { get; set; } 
    }
}