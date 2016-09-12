using System;
using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class CalendarModel : BaseModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }

        public ICollection<CalendarResourceRequirementModel> CalendarResourceRequirementModels { get; set; } 
    }
}