using System;

namespace RMS.WebAPI.Models
{
    public class ContractModel : BaseModel
    {
        public long ResourceId { get; set; }
        public long? EmployeeId { get; set; }
        public double WeeklyHours { get; set; }
        public double? BaseRateOverride { get; set; }
        public double? OvertimeModifierOverride { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}