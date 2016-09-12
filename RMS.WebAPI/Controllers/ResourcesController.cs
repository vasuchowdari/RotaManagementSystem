using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Resources.Dto;
using RMS.AppServiceLayer.Resources.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Resource")]
    public class ResourcesController : ApiController
    {
        private readonly IResourceAppService _resourceAppService;

        public ResourcesController(IResourceAppService resourceAppService)
        {
            _resourceAppService = resourceAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var resourceDto = _resourceAppService.GetById(id);
                var resourceModel = Mapper.Map<ResourceModel>(resourceDto);

                return Ok(resourceModel);
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
                var resourceDtos = _resourceAppService.GetAll();
                var resourceModels = new List<ResourceModel>();

                foreach (var resourceDto in resourceDtos)
                {
                    resourceModels.Add(Mapper.Map<ResourceModel>(resourceDto));
                }

                return Ok(resourceModels);
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
        public IHttpActionResult CreateResource(ResourceModel resourceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resourceDto = Mapper.Map<ResourceDto>(resourceModel);

                _resourceAppService.Create(resourceDto, AuthHelper.GetCurrentUserId());

                return Ok("Resource Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateResource(ResourceModel resourceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resourceDto = Mapper.Map<ResourceDto>(resourceModel);

                _resourceAppService.Update(resourceDto, AuthHelper.GetCurrentUserId());

                return Ok("Resource Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteResource(long id)
        {
            try
            {
                _resourceAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Resource Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
