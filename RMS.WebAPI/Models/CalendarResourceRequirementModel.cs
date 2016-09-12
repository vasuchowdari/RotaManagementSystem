using System;
using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class CalendarResourceRequirementModel : BaseModel
    {
        public long CalendarId { get; set; }
        public long ResourceId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CalendarModel CalendarModel { get; set; }
        public ResourceModel ResourceModel { get; set; }

        public ICollection<ShiftModel> ShiftModels { get; set; }

        public long ShiftTemplateId { get; set; }
    }
}