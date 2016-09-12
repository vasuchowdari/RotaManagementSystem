using System;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using RMS.Reoprting.Classes;
using RMS.Reoprting.ViewModels;

namespace RMS.Reoprting.Services
{
    public class ReportingService
    {
        private readonly DataContext _rmsctx;

        public ReportingService()
        {
            _rmsctx = new DataContext(ConfigurationManager.ConnectionStrings["RmsContext"].ConnectionString);
        }

        public bool GenerateMonthlyPayrollReport(string procname, Object[] parameters)
        {
            var q = _rmsctx.ExecuteQuery<MonthlyPayrollReportModel>("Exec " + procname, parameters);
            var result = q.ToList();

            return FinanceReportService.GenerateMonthlyPayrollReport(result);
        }

        public bool GenerateDailyActivityReport(string procname, Object[] parameters)
        {
            var q = _rmsctx.ExecuteQuery<DailyActivityReportModel>("Exec " + procname, parameters);
            var result = q.ToList();

            return FinanceReportService.GenerateDailyActivityReport(result);
        }
    }
}