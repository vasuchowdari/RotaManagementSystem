using System;

namespace RMS.WebAPI.Models
{
    public class BudgetPeriodModel : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public decimal ForecastTotal { get; set; }
        public decimal ActualSpendTotal { get; set; }
        public long BudgetId { get; set; }

        public BudgetModel BudgetModel { get; set; }
    }
}