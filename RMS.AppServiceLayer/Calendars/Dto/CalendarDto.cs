using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.CalendarResourceRequirements.Dto;

namespace RMS.AppServiceLayer.Calendars.Dto
{
    public class CalendarDto : BaseDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }

        public ICollection<CalendarResourceRequirementDto> CalendarResourceRequirementDtos { get; set; }
    }
}
