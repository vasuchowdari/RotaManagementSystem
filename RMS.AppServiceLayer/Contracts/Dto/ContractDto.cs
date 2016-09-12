using System;
using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.Contracts.Dto
{
    public class ContractDto : BaseDto
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
