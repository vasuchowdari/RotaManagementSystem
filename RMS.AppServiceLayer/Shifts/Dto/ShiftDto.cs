using System;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.ShiftTemplates.Dto;

namespace RMS.AppServiceLayer.Shifts.Dto
{
    public class ShiftDto : BaseDto
    {
        public bool IsAssigned { get; set; }
        public long ShiftTemplateId { get; set; }
        public long CalendarResourceRequirementId { get; set; }
        public long? EmployeeId { get; set; }
        public string Location { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ShiftTemplateDto ShiftTemplateDto { get; set; }

        public string TempCurrentStaffMember { get; set; }
        public string TempResourceTypeName { get; set; }
    }
}
