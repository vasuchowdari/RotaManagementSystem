using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.ShiftProfiles.Dto;
using RMS.AppServiceLayer.ShiftProfiles.Interfaces;
using RMS.AppServiceLayer.Zktime.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/ShiftProfile")]
    public class ShiftProfilesController : ApiController
    {
        private readonly IShiftProfileAppService _shiftProfileAppService;
        private readonly IZktRecordAppService _zktRecordAppService;

        public ShiftProfilesController(IShiftProfileAppService shiftProfileAppService,
            IZktRecordAppService zktRecordAppService)
        {
            _shiftProfileAppService = shiftProfileAppService;
            _zktRecordAppService = zktRecordAppService;
        }

        // Service Actions
        [Authorize]
        [HttpGet]
        [Route("ReturnOrderedShiftProfiles")]
        public IHttpActionResult ReturnOrderedShiftProfiles()
        {
            try
            {
                var shiftProfileModels = _shiftProfileAppService.ReturnOrderedShiftProfiles();

                return Ok(shiftProfileModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("InvalidForEmployee/{id}")]
        public IHttpActionResult GetInvalidForEmployee(long id)
        {
            try
            {
                var shiftProfileDtos = _shiftProfileAppService.GetInvalidForEmployee(id);
                var shiftProfileModels = new List<ShiftProfileModel>();

                if (shiftProfileDtos != null)
                {
                    foreach (var shiftProfileDto in shiftProfileDtos)
                    {
                        var shiftProfileModel = Mapper.Map<ShiftProfileModel>(shiftProfileDto);
                        shiftProfileModels.Add(shiftProfileModel);
                    }
                }

                return Ok(shiftProfileModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ReturnInvalidShiftProfiles")]
        public IHttpActionResult ReturnInvalidShiftProfiles()
        {
            try
            {
                var invalidShiftProfiles = _shiftProfileAppService.GetInvalidShiftProfiles();

                var shiftProfileModels = new List<ShiftProfileModel>();

                foreach (var invalidShiftProfile in invalidShiftProfiles)
                {
                    var shiftProfileModel = Mapper.Map<ShiftProfileModel>(invalidShiftProfile);
                    shiftProfileModels.Add(shiftProfileModel);
                }

                return Ok(shiftProfileModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ConsecutiveDaysWorked")]
        public IHttpActionResult ConsecutiveDaysWorked(ConsecutiveDaysWorkedModel consecutiveDaysWorkedModel)
        {
            try
            {
                var result = _shiftProfileAppService.ConsecutiveDayCalculatorDefault(consecutiveDaysWorkedModel.StaffId, consecutiveDaysWorkedModel.IsEmployee);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ConsecutveDaysByRange")]
        public IHttpActionResult ConsecutiveDaysByRange(ConsecutiveDaysWorkedModel consecutiveDaysWorkedModel)
        {
            try
            {
                var result = _shiftProfileAppService.ConsecutiveDayCalculator(
                    consecutiveDaysWorkedModel.StaffId, 
                    consecutiveDaysWorkedModel.IsEmployee,
                    consecutiveDaysWorkedModel.StartDate,
                    consecutiveDaysWorkedModel.EndDate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllForShift/{Id}")]
        public IHttpActionResult GetAllForShift(long id)
        {
            try
            {
                var shiftProfileDtos = _shiftProfileAppService.GetAllForShift(id);
                var shiftProfileModels = new List<ShiftProfileModel>();

                foreach (var shiftProfileDto in shiftProfileDtos)
                {
                    shiftProfileModels.Add(Mapper.Map<ShiftProfileModel>(shiftProfileDto));
                }

                return Ok(shiftProfileModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("CheckApproval")]
        public IHttpActionResult CheckApproval(ICollection<long> shiftIds)
        {
            try
            {
                var shiftProfileModels = new List<ShiftProfileModel>();

                foreach (var id in shiftIds)
                {
                    var shiftProfileDto = _shiftProfileAppService.CheckApproval(id);

                    if (shiftProfileDto != null)
                    {
                        var shiftProfileModel = Mapper.Map<ShiftProfileModel>(shiftProfileDto);
                        shiftProfileModels.Add(shiftProfileModel);
                    }
                }

                var newList = shiftProfileModels.OrderBy(sp => sp.StartDateTime).ToList();

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetForId(long id)
        {
            try
            {
                var shiftProfileDto = _shiftProfileAppService.GetById(id);
                var shiftProfileModel = Mapper.Map<ShiftProfileModel>(shiftProfileDto);

                return Ok(shiftProfileModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var shiftProfileDtos = _shiftProfileAppService.GetAll();
                var shiftProfileModels = new List<ShiftProfileModel>();

                foreach (var shiftProfileDto in shiftProfileDtos)
                {
                    shiftProfileModels.Add(Mapper.Map<ShiftProfileModel>(shiftProfileDto));
                }

                return Ok(shiftProfileModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // CRUD Actions
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult CreateShiftProfile(ShiftProfileModel shiftProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfileModel);

                _shiftProfileAppService.Create(shiftProfileDto, AuthHelper.GetCurrentUserId());

                // need to trip IsMatched flag on ZKT records
                _zktRecordAppService.SetIsMatched(shiftProfileModel.ZktStartDateTime, shiftProfileModel.ZktEndDateTime);

                return Ok("Shift Profile Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateShiftProfile(ShiftProfileModel shiftProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfileModel);
                var shiftProfileDto = _shiftProfileAppService.GetById(shiftProfileModel.Id);

                shiftProfileDto.Reason = shiftProfileModel.Reason;
                shiftProfileDto.Notes = shiftProfileModel.Notes;
                shiftProfileDto.ActualStartDateTime = shiftProfileModel.ActualStartDateTime;
                shiftProfileDto.ActualEndDateTime = shiftProfileModel.ActualEndDateTime;
                shiftProfileDto.IsApproved = shiftProfileModel.IsApproved;
                shiftProfileDto.IsModified = shiftProfileModel.IsModified;

                shiftProfileDto.HoursWorked =
                        CommonHelperAppService.ReturnCalculatedTimespanBetweenTwoDateTimeObjects(
                            shiftProfileDto.ActualStartDateTime, shiftProfileDto.ActualEndDateTime, 0);
                
                _shiftProfileAppService.Update(shiftProfileDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Profile Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteShiftProfile(long id)
        {
            try
            {
                _shiftProfileAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Shift Profile Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
