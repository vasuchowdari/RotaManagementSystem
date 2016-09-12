using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Calendars.Dto;
using RMS.AppServiceLayer.Companies.Dto;
using RMS.AppServiceLayer.SiteTypes.Dto;
using RMS.AppServiceLayer.SubSites.Dto;

namespace RMS.AppServiceLayer.Sites.Dto
{
    public class SiteDto : BaseDto
    {
        public string Name { get; set; }
        public int PayrollStartDate { get; set; }
        public int PayrollEndDate { get; set; }
        public long SiteTypeId { get; set; }
        public long CompanyId { get; set; }

        public SiteTypeDto SiteTypeDto { get; set; }
        public CompanyDto CompanyDto { get; set; }

        public ICollection<SubSiteDto> SubSiteDtos { get; set; }
        public ICollection<CalendarDto> CalendarDtos { get; set; } 
    }
}
