using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Shifts.Dto;
using RMS.AppServiceLayer.Shifts.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Shift")]
    public class ShiftsController : ApiController
    {
        private readonly IShiftAppService _shiftAppService;

        public ShiftsController(IShiftAppService shiftAppService)
        {
            _shiftAppService = shiftAppService;
        }


        // Service Actions
        [Authorize]
        [HttpGet]
        [Route("GetShiftLocationName/{Id}")]
        public IHttpActionResult GetShiftLocationName(long id)
        {
            try
            {
                var nameStr = _shiftAppService.GetSiteName(id);

                return Ok(nameStr);
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
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var shiftDto = _shiftAppService.GetById(id);
                var shiftModel = Mapper.Map<ShiftModel>(shiftDto);

                return Ok(shiftModel);
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
                var shiftDtos = _shiftAppService.GetAll();
                var shiftModels = new List<ShiftModel>();

                foreach (var shiftDto in shiftDtos)
                {
                    shiftModels.Add(Mapper.Map<ShiftModel>(shiftDto));
                }

                return Ok(shiftModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("CheckStaffByDate")]
        public IHttpActionResult CheckIfStaffAssignedToShiftByDateRange(CheckIfStaffAssignedToShiftDataModel dataModel)
        {
            try
            {
                var staffIdsToRemove = _shiftAppService.CheckIfStaffAssignedToShiftByDateRange(
                    dataModel.EmployeeIds, 
                    dataModel.ShiftRangeStartDate,
                    dataModel.ShiftRangeEndDate);

                return Ok(staffIdsToRemove);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetForEmployee/{Id}")]
        public IHttpActionResult GetForEmployee(long id)
        {
            try
            {
                var shiftDtos = _shiftAppService.GetByEmployeeId(id);
                var shiftModels = new List<ShiftModel>();

                foreach (var shiftDto in shiftDtos)
                {
                    shiftModels.Add(Mapper.Map<ShiftModel>(shiftDto));
                }

                return Ok(shiftModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("UnassignEmployee/{Id}")]
        public IHttpActionResult UnassignEmployee(long id)
        {
            try
            {
                var shiftDtos = _shiftAppService.GetByEmployeeId(id);
                foreach (var shiftDto in shiftDtos)
                {
                    shiftDto.EmployeeId = null;

                    _shiftAppService.Update(shiftDto, AuthHelper.GetCurrentUserId());
                }

                return Ok();
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
        public IHttpActionResult CreateShift(ShiftModel shiftModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftDto = Mapper.Map<ShiftDto>(shiftModel);

                _shiftAppService.Create(shiftDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // THIS IS A POST RATHER THAN PUT AS NOUVITA WEB SERVER WAS
        // DENYING ACCESS WITH PUT COMMAND. POST WOORKED, SO POST IT IS
        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateShift(ShiftModel shiftModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftDto = Mapper.Map<ShiftDto>(shiftModel);

                _shiftAppService.Update(shiftDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteShift(long id)
        {
            try
            {
                _shiftAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Shift Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
