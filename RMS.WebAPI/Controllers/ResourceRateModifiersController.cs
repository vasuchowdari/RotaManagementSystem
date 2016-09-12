using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.ResourceRateModifiers.Dto;
using RMS.AppServiceLayer.ResourceRateModifiers.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/RateModifier")]
    public class ResourceRateModifiersController : ApiController
    {
        private readonly IResourceRateModifierAppService _rateModifierAppService;

        public ResourceRateModifiersController(IResourceRateModifierAppService rateModifierAppService)
        {
            _rateModifierAppService = rateModifierAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var rateModifierDto = _rateModifierAppService.GetById(id);
                var rateModifierModel = Mapper.Map<ResourceRateModifierModel>(rateModifierDto);

                return Ok(rateModifierModel);
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
                var rateModifierDtos = _rateModifierAppService.GetAll();
                var rateModifierModels = new List<ResourceRateModifierModel>();

                foreach (var rateModifierDto in rateModifierDtos)
                {
                    rateModifierModels.Add(Mapper.Map<ResourceRateModifierModel>(rateModifierDto));
                }

                return Ok(rateModifierModels);
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
        public IHttpActionResult CreateRateModifier(ResourceRateModifierModel rateModifierModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rateModifierDto = Mapper.Map<ResourceRateModifierDto>(rateModifierModel);

                _rateModifierAppService.Create(rateModifierDto, AuthHelper.GetCurrentUserId());

                return Ok("Rate Modifier Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateRateModifier(ResourceRateModifierModel rateModifierModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rateModifierDto = Mapper.Map<ResourceRateModifierDto>(rateModifierModel);

                _rateModifierAppService.Update(rateModifierDto, AuthHelper.GetCurrentUserId());

                return Ok("Rate Modifier Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteRateModifier(long id)
        {
            try
            {
                _rateModifierAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Rate Modifier Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
