using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Dto;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/LeaveProfile")]
    public class PersonnelLeaveProfilesController : ApiController
    {
        private readonly IPersonnelLeaveProfileAppService _leaveProfileAppService;

        public PersonnelLeaveProfilesController(IPersonnelLeaveProfileAppService leaveProfileAppService)
        {
            _leaveProfileAppService = leaveProfileAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var leaveProfileDto = _leaveProfileAppService.GetById(id);
                var leaveProfileModel = Mapper.Map<PersonnelLeaveProfileModel>(leaveProfileDto);

                return Ok(leaveProfileModel);
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
                var leaveProfileDtos = _leaveProfileAppService.GetAll();
                var leaveProfileModels = new List<PersonnelLeaveProfileModel>();

                foreach (var leaveProfileDto in leaveProfileDtos)
                {
                    leaveProfileModels.Add(Mapper.Map<PersonnelLeaveProfileModel>(leaveProfileDto));
                }

                return Ok(leaveProfileModels);
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
        public IHttpActionResult CreateLeaveProfile(PersonnelLeaveProfileModel leaveProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaveProfileDto = Mapper.Map<PersonnelLeaveProfileDto>(leaveProfileModel);

                _leaveProfileAppService.Create(leaveProfileDto, AuthHelper.GetCurrentUserId());

                return Ok("Leave Profile Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateLeaveProfile(PersonnelLeaveProfileModel leaveProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaveProfileDto = Mapper.Map<PersonnelLeaveProfileDto>(leaveProfileModel);

                _leaveProfileAppService.Update(leaveProfileDto, AuthHelper.GetCurrentUserId());

                return Ok("Leave Profile Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteLeaveProfile(long id)
        {
            try
            {
                _leaveProfileAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Leave Profile Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
