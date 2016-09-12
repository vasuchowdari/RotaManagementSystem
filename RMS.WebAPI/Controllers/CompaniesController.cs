using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Companies.Dto;
using RMS.AppServiceLayer.Companies.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompaniesController : ApiController
    {
        private readonly ICompanyAppService _companyAppService;

        public CompaniesController(ICompanyAppService companyAppService)
        {
            _companyAppService = companyAppService;
        }

        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var companyDto = _companyAppService.GetById(id);
                var companyModel = Mapper.Map<CompanyModel>(companyDto);

                return Ok(companyModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("WithEmployees/{Id}")]
        public IHttpActionResult GetByIdWithEmployees(long id)
        {
            try
            {
                var companyDto = _companyAppService.GetByIdWithEmployees(id);
                var companyModel = Mapper.Map<CompanyModel>(companyDto);

                return Ok(companyModel);
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
                var companyDtos = _companyAppService.GetAll();
                var companyModels = new List<CompanyModel>();

                foreach (var companyDto in companyDtos)
                {
                    companyModels.Add(Mapper.Map<CompanyModel>(companyDto));
                }

                return Ok(companyModels);
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
        public IHttpActionResult CreateCompany(CompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var companyDto = Mapper.Map<CompanyDto>(companyModel);

                _companyAppService.Create(companyDto, AuthHelper.GetCurrentUserId());

                return Ok("Company Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateCompany(CompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var companyDto = Mapper.Map<CompanyDto>(companyModel);

                _companyAppService.Update(companyDto, AuthHelper.GetCurrentUserId());

                return Ok("Company Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteCompany(long id)
        {
            try
            {
                _companyAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Company Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
