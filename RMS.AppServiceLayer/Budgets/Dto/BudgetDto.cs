using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.BudgetPeriods.Dto;

namespace RMS.AppServiceLayer.Budgets.Dto
{
    public class BudgetDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ForecastTotal { get; set; }
        public decimal ActualTotal { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }

        public ICollection<BudgetPeriodDto> BudgetPeriodDtos { get; set; }
    }
}
