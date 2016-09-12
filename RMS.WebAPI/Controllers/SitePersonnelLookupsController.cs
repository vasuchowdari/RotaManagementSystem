using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.SitePersonnelLookups.Dto;
using RMS.AppServiceLayer.SitePersonnelLookups.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/SitePersonnelLookup")]
    public class SitePersonnelLookupsController : ApiController
    {
        private readonly ISitePersonnelLookupAppService _sitePersonnelLookupAppService;

        public SitePersonnelLookupsController(ISitePersonnelLookupAppService sitePersonnelLookupAppService)
        {
            _sitePersonnelLookupAppService = sitePersonnelLookupAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var sitePersonnelLookupDto = _sitePersonnelLookupAppService.GetById(id);
                var sitePersonnelLookupModel = Mapper.Map<SitePersonnelLookupModel>(sitePersonnelLookupDto);

                return Ok(sitePersonnelLookupModel);
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
                var sitePersonnelLookupDtos = _sitePersonnelLookupAppService.GetAll();
                var sitePersonnelLookupModels = new List<SitePersonnelLookupModel>();

                foreach (var sitePersonnelLookupDto in sitePersonnelLookupDtos)
                {
                    sitePersonnelLookupModels.Add(Mapper.Map<SitePersonnelLookupModel>(sitePersonnelLookupDto));
                }

                return Ok(sitePersonnelLookupModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetForEmployee/{EmployeeId}")]
        public IHttpActionResult GetForEmployee(long employeeId)
        {
            try
            {
                var sitePersonnelLookupDtos = _sitePersonnelLookupAppService.GetAllForEmployee(employeeId);
                var sitePersonnelLookupModels = new List<SitePersonnelLookupModel>();

                foreach (var sitePersonnelLookupDto in sitePersonnelLookupDtos)
                {
                    sitePersonnelLookupModels.Add(Mapper.Map<SitePersonnelLookupModel>(sitePersonnelLookupDto));
                }

                return Ok(sitePersonnelLookupModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllPlusInactiveForEmployee/{EmployeeId}")]
        public IHttpActionResult GetAllPlusInactiveForEmployee(long employeeId)
        {
            try
            {
                var sitePersonnelLookupDtos = _sitePersonnelLookupAppService.GetAllPlusInactiveForEmployee(employeeId);
                var sitePersonnelLookupModels = new List<SitePersonnelLookupModel>();

                foreach (var sitePersonnelLookupDto in sitePersonnelLookupDtos)
                {
                    sitePersonnelLookupModels.Add(Mapper.Map<SitePersonnelLookupModel>(sitePersonnelLookupDto));
                }

                return Ok(sitePersonnelLookupModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetForSite/{SiteId}")]
        public IHttpActionResult GetForSite(long siteId)
        {
            try
            {
                var sitePersonnelLookupDtos = _sitePersonnelLookupAppService.GetAllForSite(siteId);
                var sitePersonnelLookupModels = new List<SitePersonnelLookupModel>();

                foreach (var sitePersonnelLookupDto in sitePersonnelLookupDtos)
                {
                    sitePersonnelLookupModels.Add(Mapper.Map<SitePersonnelLookupModel>(sitePersonnelLookupDto));
                }

                return Ok(sitePersonnelLookupModels);
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
        public IHttpActionResult CreateSitePersonnelLookup(SitePersonnelLookupModel sitePersonnelLookupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var sitePersonnelLookupDto = Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookupModel);

                _sitePersonnelLookupAppService.Create(sitePersonnelLookupDto, AuthHelper.GetCurrentUserId());

                return Ok("Access record was successfully created.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        [Authorize]
        [HttpPost]
        [Route("CreateBatch")]
        public IHttpActionResult CreateSitePersonnelLookupBatch(ICollection<SitePersonnelLookupModel> modelList)
        {
            try
            {
                foreach (var model in modelList)
                {
                    var sitePersonnelLookupDto = Mapper.Map<SitePersonnelLookupDto>(model);
                    _sitePersonnelLookupAppService.Create(sitePersonnelLookupDto, AuthHelper.GetCurrentUserId());
                }

                return Ok("Access records were successfully created.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateSitePersonnelLookup(SitePersonnelLookupModel sitePersonnelLookupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var sitePersonnelLookupDto = Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookupModel);

                _sitePersonnelLookupAppService.Update(sitePersonnelLookupDto, AuthHelper.GetCurrentUserId());

                return Ok("Access record was successfully updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateBatch")]
        public IHttpActionResult UpdateSiteAccessRecordBatch(ICollection<SitePersonnelLookupModel> modelList)
        {
            try
            {
                foreach (var model in modelList)
                {
                    var sitePersonnelLookupDto = Mapper.Map<SitePersonnelLookupDto>(model);
                    _sitePersonnelLookupAppService.Update(sitePersonnelLookupDto, AuthHelper.GetCurrentUserId());
                }

                return Ok("Access records were successfully updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteSitePersonnelLookup(long id)
        {
            try
            {
                _sitePersonnelLookupAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Access record was successfully deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
