using System;

namespace RMS.WebAPI.Models
{
    public class ShiftModel : BaseModel
    {
        public bool IsAssigned { get; set; }
        public long ShiftTemplateId { get; set; }
        public long CalendarResourceRequirementId { get; set; }
        public long? EmployeeId { get; set; }
        public string Location { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ShiftTemplateModel ShiftTemplateModel { get; set; }
        public CalendarResourceRequirementModel CalendarResourceRequirementModel { get; set; }

        public string TempCurrentStaffMember { get; set; }
        public string TempResourceTypeName { get; set; }
    }
}