using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Shifts.Dto;
using RMS.AppServiceLayer.ShiftTemplates.Dto;
using RMS.AppServiceLayer.ShiftTemplates.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/ShiftTemplate")]
    public class ShiftTemplatesController : ApiController
    {
        private readonly IShiftTemplateAppService _shiftTemplateAppService;

        public ShiftTemplatesController(IShiftTemplateAppService shiftTemplateAppService)
        {
            _shiftTemplateAppService = shiftTemplateAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var shiftTemplateDto = _shiftTemplateAppService.GetById(id);
                var shiftTemplateModel = Mapper.Map<ShiftTemplateModel>(shiftTemplateDto);

                return Ok(shiftTemplateModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetBySearchCriteria")]
        public IHttpActionResult GetBySearchCriteria(ShiftTemplateSearchCriteriaModel model)
        {
            try
            {
                var shiftTemplateDtos = _shiftTemplateAppService.GetBySearchCriteria(model.ResourceId, model.SiteId, model.SubSiteId);
                var shiftTemplateModels = new List<ShiftTemplateModel>();

                foreach (var shiftTemplateDto in shiftTemplateDtos)
                {
                    shiftTemplateModels.Add(Mapper.Map<ShiftTemplateModel>(shiftTemplateDto));
                }

                return Ok(shiftTemplateModels);
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
                var shiftTemplateDtos = _shiftTemplateAppService.GetAll();
                var shiftTemplateModels = new List<ShiftTemplateModel>();

                foreach (var shiftTemplateDto in shiftTemplateDtos)
                {
                    shiftTemplateModels.Add(Mapper.Map<ShiftTemplateModel>(shiftTemplateDto));
                }

                return Ok(shiftTemplateModels);
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
                var shiftTemplateDtos = _shiftTemplateAppService.GetForSite(siteId);
                var shiftTemplateModels = new List<ShiftTemplateModel>();

                foreach (var shiftTemplateDto in shiftTemplateDtos)
                {
                    shiftTemplateModels.Add(Mapper.Map<ShiftTemplateModel>(shiftTemplateDto));
                }

                return Ok(shiftTemplateModels);
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
        public IHttpActionResult CreateShiftTemplate(ShiftTemplateModel shiftTemplateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftTemplateDto = Mapper.Map<ShiftTemplateDto>(shiftTemplateModel);

                _shiftTemplateAppService.Create(shiftTemplateDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Template Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("CreateBatch")]
        public IHttpActionResult BatchCreateShiftTemplates(ICollection<ShiftTemplateModel> shiftTemplateModelCollection)
        {
            try
            {
                foreach (var shiftTemplateModel in shiftTemplateModelCollection)
                {
                    var shiftTemplateDto = Mapper.Map<ShiftTemplateDto>(shiftTemplateModel);

                    _shiftTemplateAppService.Create(shiftTemplateDto, AuthHelper.GetCurrentUserId());                    
                }

                return Ok("Shift Templates Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateShiftTemplate(ShiftTemplateModel shiftTemplateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftTemplateDto = Mapper.Map<ShiftTemplateDto>(shiftTemplateModel);

                _shiftTemplateAppService.Update(shiftTemplateDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Template Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteShiftTemplate(long id)
        {
            try
            {
                _shiftTemplateAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Shift Template Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
