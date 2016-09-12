using System;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Budgets.Dto;

namespace RMS.AppServiceLayer.BudgetPeriods.Dto
{
    public class BudgetPeriodDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public decimal ForecastTotal { get; set; }
        public decimal ActualSpendTotal { get; set; }
        public long BudgetId { get; set; }

        public BudgetDto BudgetDto { get; set; }
    }
}
