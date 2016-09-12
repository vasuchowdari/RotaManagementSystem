using System;

namespace RMS.Core
{
    public class Shift : BaseEntity
    {
        public bool IsAssigned { get; set; }
        public long ShiftTemplateId { get; set; }
        public long CalendarResourceRequirementId { get; set; }
        public long? EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ShiftTemplate ShiftTemplate { get; set; }
        public CalendarResourceRequirement CalendarResourceRequirement { get; set; }
    }
}
