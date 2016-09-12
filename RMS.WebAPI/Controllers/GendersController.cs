using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Genders.Dto;
using RMS.AppServiceLayer.Genders.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Gender")]
    public class GendersController : ApiController
    {
        private readonly IGenderAppService _genderAppService;

        public GendersController(IGenderAppService genderAppService)
        {
            _genderAppService = genderAppService;
        }

        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var genderDto = _genderAppService.GetById(id);
                var genderModel = Mapper.Map<GenderModel>(genderDto);

                return Ok(genderModel);
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
                var genderDtos = _genderAppService.GetAll();
                var genderModels = new List<GenderModel>();

                foreach (var genderDto in genderDtos)
                {
                    genderModels.Add(Mapper.Map<GenderModel>(genderDto));
                }

                return Ok(genderModels);
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
        public IHttpActionResult CreateGender(GenderModel genderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genderDto = Mapper.Map<GenderDto>(genderModel);

                _genderAppService.Create(genderDto, AuthHelper.GetCurrentUserId());

                return Ok("Gender Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateGender(GenderModel genderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genderDto = Mapper.Map<GenderDto>(genderModel);

                _genderAppService.Update(genderDto, AuthHelper.GetCurrentUserId());

                return Ok("Gender Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteGender(long id)
        {
            try
            {
                _genderAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Gender Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
