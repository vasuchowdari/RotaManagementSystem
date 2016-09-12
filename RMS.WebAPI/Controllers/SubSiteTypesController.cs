using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.SubSiteTypes.Dto;
using RMS.AppServiceLayer.SubSiteTypes.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/SubSiteType")]
    public class SubSiteTypesController : ApiController
    {
        private readonly ISubSiteTypeAppService _subSiteTypeAppService;

        public SubSiteTypesController(ISubSiteTypeAppService subSiteTypeAppService)
        {
            _subSiteTypeAppService = subSiteTypeAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var subSiteTypeDto = _subSiteTypeAppService.GetById(id);
                var subSiteTypeModel = Mapper.Map<SubSiteTypeModel>(subSiteTypeDto);

                return Ok(subSiteTypeModel);
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
                var subSiteTypeDtos = _subSiteTypeAppService.GetAll();
                var subSiteTypeModels = new List<SubSiteTypeModel>();

                foreach (var subSiteTypeDto in subSiteTypeDtos)
                {
                    subSiteTypeModels.Add(Mapper.Map<SubSiteTypeModel>(subSiteTypeDto));
                }

                return Ok(subSiteTypeModels);
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
        public IHttpActionResult CreateSubSiteType(SubSiteTypeModel subSiteTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var subSiteTypeDto = Mapper.Map<SubSiteTypeDto>(subSiteTypeModel);

                _subSiteTypeAppService.Create(subSiteTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Sub-Site Type Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateSubSiteType(SubSiteTypeModel subSiteTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var subSiteTypeDto = Mapper.Map<SubSiteTypeDto>(subSiteTypeModel);

                _subSiteTypeAppService.Update(subSiteTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Sub-Site Type Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteSubSiteType(long id)
        {
            try
            {
                _subSiteTypeAppService.Delete(id, AuthHelper.GetCurrentUserId());
                
                return Ok("Sub-Site Type Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
