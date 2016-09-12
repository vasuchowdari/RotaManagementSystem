using System;
using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class BudgetModel : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ForecastTotal { get; set; }
        public decimal ActualTotal { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }

        public ICollection<BudgetPeriodModel> BudgetPeriodModels { get; set; }
    }
}