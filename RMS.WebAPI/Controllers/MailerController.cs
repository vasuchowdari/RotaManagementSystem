using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Mailers.Dto;
using RMS.AppServiceLayer.Mailers.Interfaces;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Mailer")]
    public class MailerController : ApiController
    {
        private readonly IMailerAppService _mailerAppService;

        public MailerController(IMailerAppService mailerAppService)
        {
            _mailerAppService = mailerAppService;
        }


        //// Service Actions
        //[Authorize]
        //[HttpPost]
        //[Route("LeaveRequest")]
        //public IHttpActionResult SendLeaveRequestEmail(LeaveRequestFormModel leaveRequestFormModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var leaveRequestFormDto = Mapper.Map<LeaveRequestFormDto>(leaveRequestFormModel);

        //        _mailerAppService.SendLeaveRequestEmail(leaveRequestFormDto);

        //        return Ok("Leave Request Form submitted.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("TimeAdjustment")]
        //public IHttpActionResult SendTimeAdjustmentFormEmail(TimeAdjustmentFormModel timeAdjustmentFormModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var timeAdjustmentFormDto = Mapper.Map<TimeAdjustmentFormDto>(timeAdjustmentFormModel);

        //        _mailerAppService.SendTimeAdjustmentForm(timeAdjustmentFormDto);

        //        return Ok("Time Correction Form submitted.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
