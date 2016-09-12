using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Calendars.Dto;
using RMS.AppServiceLayer.Resources.Dto;
using RMS.AppServiceLayer.Shifts.Dto;

namespace RMS.AppServiceLayer.CalendarResourceRequirements.Dto
{
    public class CalendarResourceRequirementDto : BaseDto
    {
        public long CalendarId { get; set; }
        public long ResourceId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CalendarDto CalendarDto { get; set; }
        public ResourceDto ResourceDto { get; set; }

        public ICollection<ShiftDto> ShiftDtos { get; set; }

        public long ShiftTemplateId { get; set; }
    }
}
