using System;
using RMS.AppServiceLayer.Reports.Dto;

namespace RMS.AppServiceLayer.Reports.Interfaces
{
    public interface IReportAppService : IDisposable
    {
        // Finance Reports
        bool RunMonthlyPayrollReport(MonthlyPayrollReportParamsDto paramsDto);
        bool RunDailyActivityReport(DailyActivityReportParamsDto paramsDto);

        // HR Reports

    }
}
