using System;

namespace RMS.WebAPI.Models
{
    public class ShiftTemplateModel : BaseModel
    {
        public string Name { get; set; }
        public decimal ShiftRate { get; set; }
        public long ShiftTypeId { get; set; }
        public long ResourceId { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public double UnpaidBreakDuration { get; set; }

        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
        public bool Sun { get; set; }

        public ShiftTypeModel ShiftTypeModel { get; set; }
        public ResourceModel ResourceModel { get; set; }
    }
}