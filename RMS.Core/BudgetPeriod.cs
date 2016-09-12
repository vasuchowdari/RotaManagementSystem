using System;

namespace RMS.Core
{
    public class BudgetPeriod : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public decimal ForecastTotal { get; set; }
        public decimal ActualSpendTotal { get; set; }
        public long BudgetId { get; set; }

        public Budget Budget { get; set; }
    }
}
