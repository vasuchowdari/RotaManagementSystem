using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Calendars.Dto;

namespace RMS.AppServiceLayer.SubSites.Dto
{
    public class SubSiteDto : BaseDto
    {
        public string Name { get; set; }
        public long SubSiteTypeId { get; set; }
        public long SiteId { get; set; }

        public ICollection<CalendarDto> CalendarDtos { get; set; } 
    }
}
