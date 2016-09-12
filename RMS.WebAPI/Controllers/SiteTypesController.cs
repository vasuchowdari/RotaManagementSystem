using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.SiteTypes.Dto;
using RMS.AppServiceLayer.SiteTypes.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/SiteType")]
    public class SiteTypesController : ApiController
    {
        private readonly ISiteTypeAppService _siteTypeAppService;

        public SiteTypesController(ISiteTypeAppService siteTypeAppService)
        {
            _siteTypeAppService = siteTypeAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var siteTypeDto = _siteTypeAppService.GetById(id);
                var siteTypeModel = Mapper.Map<SiteTypeModel>(siteTypeDto);

                return Ok(siteTypeModel);
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
                var siteTypeDtos = _siteTypeAppService.GetAll();
                var siteTypeModels = new List<SiteTypeModel>();

                foreach (var siteTypeDto in siteTypeDtos)
                {
                    siteTypeModels.Add(Mapper.Map<SiteTypeModel>(siteTypeDto));
                }

                return Ok(siteTypeModels);
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
        public IHttpActionResult CreateSiteType(SiteTypeModel siteTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var siteTypeDto = Mapper.Map<SiteTypeDto>(siteTypeModel);

                _siteTypeAppService.Create(siteTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Site Type Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateSiteType(SiteTypeModel siteTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var siteTypeDto = Mapper.Map<SiteTypeDto>(siteTypeModel);

                _siteTypeAppService.Update(siteTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Site Type Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteSiteType(long id)
        {
            try
            {
                _siteTypeAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Site Type Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
