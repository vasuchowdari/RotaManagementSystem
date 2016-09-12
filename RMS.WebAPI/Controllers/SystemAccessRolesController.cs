using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.SystemAccessRoles.Dto;
using RMS.AppServiceLayer.SystemAccessRoles.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/SystemAccessRole")]
    public class SystemAccessRolesController : ApiController
    {
        private readonly ISystemAccessRoleAppService _systemAccessRoleAppService;

        public SystemAccessRolesController(ISystemAccessRoleAppService systemAccessRoleAppService)
        {
            _systemAccessRoleAppService = systemAccessRoleAppService;
        }

        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var systemAccessRoleDto = _systemAccessRoleAppService.GetById(id);
                var systemAccessRoleModel = Mapper.Map<SystemAccessRoleModel>(systemAccessRoleDto);

                return Ok(systemAccessRoleModel);
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
                var systemAccessRoleDtos = _systemAccessRoleAppService.GetAll();
                var systemAccessRoleModels = new List<SystemAccessRoleModel>();

                foreach (var systemAccessRoleDto in systemAccessRoleDtos)
                {
                    systemAccessRoleModels.Add(Mapper.Map<SystemAccessRoleModel>(systemAccessRoleDto));
                }

                return Ok(systemAccessRoleModels);
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
        public IHttpActionResult CreateSystemAccessRole(SystemAccessRoleModel systemAccessRoleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var systemAccessRoleDto = Mapper.Map<SystemAccessRoleDto>(systemAccessRoleModel);

                _systemAccessRoleAppService.Create(systemAccessRoleDto, AuthHelper.GetCurrentUserId());

                return Ok("System Access Role Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateSystemAccessRole(SystemAccessRoleModel systemAccessRoleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var systemAccessRoleDto = Mapper.Map<SystemAccessRoleDto>(systemAccessRoleModel);

                _systemAccessRoleAppService.Update(systemAccessRoleDto, AuthHelper.GetCurrentUserId());

                return Ok("System Access Role Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteSystemAccessRole(long id)
        {
            try
            {
                _systemAccessRoleAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("System Access Role Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
