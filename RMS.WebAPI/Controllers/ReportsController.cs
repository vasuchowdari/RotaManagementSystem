using System;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Reports.Dto;
using RMS.AppServiceLayer.Reports.Interfaces;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Report")]
    public class ReportsController : ApiController
    {
        private readonly IReportAppService _reportAppService;

        public ReportsController(IReportAppService reportAppService)
        {
            _reportAppService = reportAppService;
        }

        [Authorize]
        [HttpPost]
        [Route("Payroll/Monthly")]
        public IHttpActionResult RunMonthlyPayrollReport(MonthlyPayrollReportParamsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var paramsDto = Mapper.Map<MonthlyPayrollReportParamsDto>(model);
                var fileReadyFlag = _reportAppService.RunMonthlyPayrollReport(paramsDto);

                return Ok(fileReadyFlag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("DailyActivity")]
        public IHttpActionResult RunDailyActivityReport(DailyActivityReportParamsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var paramsDto = Mapper.Map<DailyActivityReportParamsDto>(model);
                var fileReadyFlag = _reportAppService.RunDailyActivityReport(paramsDto);

                return Ok(fileReadyFlag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
