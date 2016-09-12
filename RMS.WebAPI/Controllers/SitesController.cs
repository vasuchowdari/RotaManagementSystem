using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Sites.Dto;
using RMS.AppServiceLayer.Sites.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Site")]
    public class SitesController : ApiController
    {
        private readonly ISiteAppService _siteAppService;

        public SitesController(ISiteAppService siteAppService)
        {
            _siteAppService = siteAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long companyId)
        {
            try
            {
                var siteDto = _siteAppService.GetById(companyId);
                var siteModel = Mapper.Map<SiteModel>(siteDto);

                return Ok(siteModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Company/{CompanyId}")]
        public IHttpActionResult GetForCompany(long companyId)
        {
            try
            {
                //var siteDto = _siteAppService.GetById(companyId);
                var siteDtos = _siteAppService.GetForCompany(companyId);
                //var siteModel = Mapper.Map<SiteModel>(siteDto);

                if (siteDtos.Any())
                {
                    var siteModels = new List<SiteModel>();

                    foreach (var siteDto in siteDtos)
                    {
                        siteModels.Add(Mapper.Map<SiteModel>(siteDto));
                    }

                    return Ok(siteModels);
                }

                return BadRequest();
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
                var siteDtos = _siteAppService.GetAll();
                var siteModels = new List<SiteModel>();

                foreach (var siteDto in siteDtos)
                {
                    siteModels.Add(Mapper.Map<SiteModel>(siteDto));
                }

                return Ok(siteModels);
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
        public IHttpActionResult CreateSite(SiteModel siteModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var siteDto = Mapper.Map<SiteDto>(siteModel);

                _siteAppService.Create(siteDto, AuthHelper.GetCurrentUserId());

                return Ok("Site Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateSite(SiteModel siteModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var siteDto = Mapper.Map<SiteDto>(siteModel);

                _siteAppService.Update(siteDto, AuthHelper.GetCurrentUserId());

                return Ok("Site Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteSite(long id)
        {
            try
            {
                _siteAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Site Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
