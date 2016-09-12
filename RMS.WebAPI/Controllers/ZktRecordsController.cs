using System;
using System.Web.Http;
using RMS.AppServiceLayer.Zktime.Interfaces;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/ZktRecords")]
    public class ZktRecordsController : ApiController
    {
        private readonly IZktRecordAppService _zktAppService;

        public ZktRecordsController(IZktRecordAppService zktAppService)
        {
            _zktAppService = zktAppService;
        }


        // Service Action
        [Authorize]
        [HttpPost]
        [Route("GetUnmatchedByName")]
        public IHttpActionResult GetUnmatchedByName(ZktUnmatchedByEmployeeModel model)
        {
            try
            {
                var unmatchedClockingRecords = _zktAppService.GetUnmatchedByEmployeeName(model.EmployeeName, model.StartDateTime, model.EndDateTime);

                return Ok(unmatchedClockingRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
