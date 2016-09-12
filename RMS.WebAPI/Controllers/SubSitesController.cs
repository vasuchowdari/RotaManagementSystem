using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.SubSites.Dto;
using RMS.AppServiceLayer.SubSites.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/SubSite")]
    public class SubSitesController : ApiController
    {
        private readonly ISubSiteAppService _subSiteAppService;

        public SubSitesController(ISubSiteAppService subSiteAppService)
        {
            _subSiteAppService = subSiteAppService;
        }


        // Repo Methods
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var subSiteDto = _subSiteAppService.GetById(id);
                var subSiteModel = Mapper.Map<SubSiteModel>(subSiteDto);

                return Ok(subSiteModel);
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
                var subSiteDtos = _subSiteAppService.GetAll();
                var subSiteModels = new List<SubSiteModel>();

                foreach (var subSiteDto in subSiteDtos)
                {
                    subSiteModels.Add(Mapper.Map<SubSiteModel>(subSiteDto));
                }

                return Ok(subSiteModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // CRUD
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult CreateSubSite(SubSiteModel subSiteModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var subSiteDto = Mapper.Map<SubSiteDto>(subSiteModel);

                _subSiteAppService.Create(subSiteDto, AuthHelper.GetCurrentUserId());

                return Ok("Sub-Site Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateSubSite(SubSiteModel subSiteModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var subSiteDto = Mapper.Map<SubSiteDto>(subSiteModel);

                _subSiteAppService.Update(subSiteDto, AuthHelper.GetCurrentUserId());

                return Ok("Sub-Site Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteSubSite(long id)
        {
            try
            {
                _subSiteAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Sub-Site Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
