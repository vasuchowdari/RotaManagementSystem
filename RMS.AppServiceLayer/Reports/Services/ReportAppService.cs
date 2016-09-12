using System;
using RMS.AppServiceLayer.Reports.Dto;
using RMS.AppServiceLayer.Reports.Interfaces;
using RMS.Infrastructure.EF;
using RMS.Reoprting.Services;

namespace RMS.AppServiceLayer.Reports.Services
{
    public class ReportAppService : IReportAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly ReportingService _reportingService;

        public ReportAppService()
        {
            _reportingService = new ReportingService();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public bool RunMonthlyPayrollReport(MonthlyPayrollReportParamsDto paramsDto)
        {
            var startTs = new TimeSpan(0, 0, 0);
            var endTs = new TimeSpan(23, 59, 59);

            // default times for date ranges
            paramsDto.StartDate = paramsDto.StartDate.Date + startTs;
            paramsDto.EndDate = paramsDto.EndDate.Date + endTs;

            var parameters = new Object[2];
            parameters[0] = paramsDto.StartDate;
            parameters[1] = paramsDto.EndDate;

            //parameters[2] = paramsDto.SiteName ?? string.Empty;
            //parameters[3] = paramsDto.SubSiteName ?? string.Empty;
            //parameters[4] = paramsDto.Firstname ?? string.Empty;
            //parameters[5] = paramsDto.Lastname ?? string.Empty;
            //parameters[6] = paramsDto.RoleName ?? string.Empty;

            return _reportingService.GenerateMonthlyPayrollReport("dbo.MonthlyPayrollReport {0}, {1}", parameters);
            //return _reportingService.GenerateMonthlyPayrollReport("dbo.MonthlyPayrollReport {0}, {1}, {2}, {3}, {4}, {5}, {6}", parameters);
        }

        public bool RunDailyActivityReport(DailyActivityReportParamsDto paramsDto)
        {
            var startTs = new TimeSpan(0, 0, 0);
            var endTs = new TimeSpan(23, 59, 59);

            // default times for date ranges
            paramsDto.StartDate = paramsDto.StartDate.Date + startTs;
            paramsDto.EndDate = paramsDto.EndDate.Date + endTs;
            
            var parameters = new Object[7];
            parameters[0] = paramsDto.StartDate;
            parameters[1] = paramsDto.EndDate;
            parameters[2] = paramsDto.SiteName ?? string.Empty;
            parameters[3] = paramsDto.SubSiteName ?? string.Empty;
            parameters[4] = paramsDto.Firstname ?? string.Empty;
            parameters[5] = paramsDto.Lastname ?? string.Empty;
            parameters[6] = paramsDto.RoleName ?? string.Empty;

            return _reportingService.GenerateDailyActivityReport("dbo.DailyActivityReport {0}, {1}, {2}, {3}, {4}, {5}, {6}", parameters);
        }
    }
}
