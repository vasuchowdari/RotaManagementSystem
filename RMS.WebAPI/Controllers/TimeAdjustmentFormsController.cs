using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Mailers.Interfaces;
using RMS.AppServiceLayer.ShiftProfiles.Interfaces;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;
using RMS.AppServiceLayer.TimeFormAdjustments.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/TimeAdjustmentForm")]
    public class TimeAdjustmentFormsController : ApiController
    {
        private readonly ITimeAdjustmentFormAppService _timeAdjustmentFormAppService;
        private readonly IShiftProfileAppService _shiftProfileAppService;
        private readonly IMailerAppService _mailerAppService;

        public TimeAdjustmentFormsController(
            ITimeAdjustmentFormAppService timeAdjustmentFormAppService,
            IMailerAppService mailerAppService,
            IShiftProfileAppService shiftProfileAppService)
        {
            _timeAdjustmentFormAppService = timeAdjustmentFormAppService;
            _mailerAppService = mailerAppService;
            _shiftProfileAppService = shiftProfileAppService;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllUnapproved")]
        public IHttpActionResult GetAllUnapproved()
        {
            try
            {
                var timeAdjustmentFormDtos = _timeAdjustmentFormAppService.GetAllUnapproved();
                var timeAdjustmentFormModels = new List<TimeAdjustmentFormModel>();

                foreach (var timeAdjustmentFormDto in timeAdjustmentFormDtos)
                {
                    timeAdjustmentFormModels.Add(Mapper.Map<TimeAdjustmentFormModel>(timeAdjustmentFormDto));
                }

                return Ok(timeAdjustmentFormModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetForIds")]
        public IHttpActionResult GetForIds(List<long> staffIds)
        {
            try
            {
                var timeAdjustmentFormDtos = _timeAdjustmentFormAppService.GetForIds(staffIds);
                var timeAdjustmentFormModels = new List<TimeAdjustmentFormModel>();

                foreach (var timeAdjustmentFormDto in timeAdjustmentFormDtos)
                {
                    timeAdjustmentFormModels.Add(Mapper.Map<TimeAdjustmentFormModel>(timeAdjustmentFormDto));
                }

                return Ok(timeAdjustmentFormModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(TimeAdjustmentFormModel timeAdjustmentFormModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                timeAdjustmentFormModel.IsActive = true;
                
                var timeAdjustmentFormDto = Mapper.Map<TimeAdjustmentFormDto>(timeAdjustmentFormModel);
                
                _timeAdjustmentFormAppService.Create(timeAdjustmentFormDto, AuthHelper.GetCurrentUserId());

                 //send an email out to the manager
                _mailerAppService.SendTimeAdjustmentForm(timeAdjustmentFormDto);

                return Ok("Time Adjustment Form submitted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateFormAndShiftProfile")]
        public IHttpActionResult UpdateFormAndShiftProfile(TimeAdjustmentFormAndShiftProfileUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // TAF
                var tafDto = _timeAdjustmentFormAppService.GetById(model.TimeAdjustmentFormId);
                tafDto.IsManagerApproved = model.IsManagerApproved;
                tafDto.IsAdminApproved = model.IsAdminApproved;
                _timeAdjustmentFormAppService.Update(tafDto, AuthHelper.GetCurrentUserId());

                // Shift Profile
                var shiftProfileDto = _shiftProfileAppService.GetById(model.ShiftProfileId);
                shiftProfileDto.Reason = model.Reason;
                shiftProfileDto.Notes = model.Notes;

                if (model.IsAdminApproved)
                {
                    shiftProfileDto.IsApproved = true;
                    shiftProfileDto.ActualStartDateTime = model.ActualStartDateTime;
                    shiftProfileDto.ActualEndDateTime = model.ActualEndDateTime;

                    shiftProfileDto.HoursWorked =
                        CommonHelperAppService.ReturnCalculatedTimespanBetweenTwoDateTimeObjects(
                            shiftProfileDto.ActualStartDateTime, shiftProfileDto.ActualEndDateTime, 0);
                }

                _shiftProfileAppService.Update(shiftProfileDto, AuthHelper.GetCurrentUserId());

                return Ok("Success.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
