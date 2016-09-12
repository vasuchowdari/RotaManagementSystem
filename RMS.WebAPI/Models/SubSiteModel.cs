using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class SubSiteModel : BaseModel
    {
        public string Name { get; set; }
        public long SubSiteTypeId { get; set; }
        public long SiteId { get; set; }

        public ICollection<CalendarModel> CalendarModels { get; set; } 
    }
}