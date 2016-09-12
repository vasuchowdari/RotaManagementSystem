using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.EmployeeTypes.Dto;
using RMS.AppServiceLayer.EmployeeTypes.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/EmployeeType")]
    public class EmployeeTypesController : ApiController
    {
        private readonly IEmployeeTypeAppService _employeeTypeAppService;

        public EmployeeTypesController(IEmployeeTypeAppService employeeTypeAppService)
        {
            _employeeTypeAppService = employeeTypeAppService;
        }

        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var employeeTypeDto = _employeeTypeAppService.GetById(id);
                var employeeTypeModel = Mapper.Map<EmployeeTypeModel>(employeeTypeDto);

                return Ok(employeeTypeModel);
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
                var employeeTypeDtos = _employeeTypeAppService.GetAll();
                var employeeTypeModels = new List<EmployeeTypeModel>();

                foreach (var employeeTypeDto in employeeTypeDtos)
                {
                    employeeTypeModels.Add(Mapper.Map<EmployeeTypeModel>(employeeTypeDto));
                }

                return Ok(employeeTypeModels);
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
        public IHttpActionResult CreateEmployeeType(EmployeeTypeModel employeeTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employeeTypeDto = Mapper.Map<EmployeeTypeDto>(employeeTypeModel);

                _employeeTypeAppService.Create(employeeTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Employee Type Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateEmployeeType(EmployeeTypeModel employeeTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employeeTypeDto = Mapper.Map<EmployeeTypeDto>(employeeTypeModel);

                _employeeTypeAppService.Update(employeeTypeDto, AuthHelper.GetCurrentUserId());

                return Ok("Employee Type Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteEmployeeType(long id)
        {
            try
            {
                _employeeTypeAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Employee Type Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
