using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/SystemConfig")]
    public class UserDefinedSystemConfigsController : ApiController
    {
        private readonly IUserDefinedSystemConfigAppService _userDefinedSystemConfigAppService;

        public UserDefinedSystemConfigsController(IUserDefinedSystemConfigAppService userDefinedSystemConfigAppService)
        {
            _userDefinedSystemConfigAppService = userDefinedSystemConfigAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var userDefinedSystemConfigDtos = _userDefinedSystemConfigAppService.GetAll();
                var userDefinedSystemConfigModels = new List<UserDefinedSystemConfigModel>();

                foreach (var systemConfigDto in userDefinedSystemConfigDtos)
                {
                    userDefinedSystemConfigModels.Add(Mapper.Map<UserDefinedSystemConfigModel>(systemConfigDto));
                }

                return Ok(userDefinedSystemConfigModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // CRUD Actions
        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateSystemConfig(UserDefinedSystemConfigModel udscModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var udscDto = Mapper.Map<UserDefinedSystemConfigDto>(udscModel);

                _userDefinedSystemConfigAppService.Update(udscDto, AuthHelper.GetCurrentUserId());

                return Ok("System Configuration Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
