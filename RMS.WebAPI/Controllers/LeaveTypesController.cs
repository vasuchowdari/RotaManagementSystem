using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.LeaveTypes.Dto;
using RMS.AppServiceLayer.LeaveTypes.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/LeaveType")]
    public class LeaveTypesController : ApiController
    {
        private readonly ILeaveTypeAppService _leaveTypeAppService;

        public LeaveTypesController(ILeaveTypeAppService leaveTypeAppService)
        {
            _leaveTypeAppService = leaveTypeAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var leaveTypeDto = _leaveTypeAppService.GetById(id);
                var leaveTypeModel = Mapper.Map<LeaveTypeModel>(leaveTypeDto);

                return Ok(leaveTypeModel);
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
                var leaveTypeDtos = _leaveTypeAppService.GetAll();
                var leaveTypeModels = new List<LeaveTypeModel>();

                foreach (var leaveTypeDto in leaveTypeDtos)
                {
                    leaveTypeModels.Add(Mapper.Map<LeaveTypeModel>(leaveTypeDto));
                }

                return Ok(leaveTypeModels);

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
        public IHttpActionResult CreateLeaveType(LeaveTypeModel leaveTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaveTypeDto = Mapper.Map<LeaveTypeDto>(leaveTypeModel);

                _leaveTypeAppService.Create(leaveTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Leave Type Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateLeaveType(LeaveTypeModel leaveTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaveTypeDto = Mapper.Map<LeaveTypeDto>(leaveTypeModel);

                _leaveTypeAppService.Update(leaveTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Leave Type Update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteLeaveType(long id)
        {
            try
            {
                _leaveTypeAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Leave Type Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
