using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.ShiftTypes.Dto;
using RMS.AppServiceLayer.ShiftTypes.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/ShiftType")]
    public class ShiftTypesController : ApiController
    {
        private readonly IShiftTypeAppService _shiftTypeAppService;

        public ShiftTypesController(IShiftTypeAppService shiftTypeAppService)
        {
            _shiftTypeAppService = shiftTypeAppService;
        }

        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var shiftTypeDto = _shiftTypeAppService.GetById(id);
                var shiftTypeModel = Mapper.Map<ShiftTypeModel>(shiftTypeDto);

                return Ok(shiftTypeModel);
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
                var shiftTypeDtos = _shiftTypeAppService.GetAll();
                var shiftTypeModels = new List<ShiftTypeModel>();

                foreach (var shiftTypeDto in shiftTypeDtos)
                {
                    shiftTypeModels.Add(Mapper.Map<ShiftTypeModel>(shiftTypeDto));
                }

                return Ok(shiftTypeModels);

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
        public IHttpActionResult CreateShiftType(ShiftTypeModel shiftTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftTypeDto = Mapper.Map<ShiftTypeDto>(shiftTypeModel);

                _shiftTypeAppService.Create(shiftTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Type Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateShiftType(ShiftTypeModel shiftTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shiftTypeDto = Mapper.Map<ShiftTypeDto>(shiftTypeModel);

                _shiftTypeAppService.Update(shiftTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Shift Type Update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteShiftType(long id)
        {
            try
            {
                _shiftTypeAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Shift Type Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
