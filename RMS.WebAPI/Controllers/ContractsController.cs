using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Contracts.Dto;
using RMS.AppServiceLayer.Contracts.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Contract")]
    public class ContractsController : ApiController
    {
        private readonly IContractAppService _contractAppService;

        public ContractsController(IContractAppService contractAppService)
        {
            _contractAppService = contractAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var contractDto = _contractAppService.GetById(id);
                var contractModel = Mapper.Map<ContractModel>(contractDto);

                return Ok(contractModel);
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
                var contractDtos = _contractAppService.GetAll();
                var contractModels = new List<ContractModel>();

                foreach (var contractDto in contractDtos)
                {
                    contractModels.Add(Mapper.Map<ContractModel>(contractDto));
                }

                return Ok(contractModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetContractForEmployee/{EmployeeId}")]
        public IHttpActionResult GetContractForEmployee(long employeeId)
        {
            try
            {
                var contractDto = _contractAppService.GetContractForEmployee(employeeId);

                if (contractDto != null)
                {
                    var contractModel = Mapper.Map<ContractModel>(contractDto);

                    return Ok(contractModel);
                }

                return BadRequest();
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
        public IHttpActionResult CreateContract(ContractModel contractModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var contractDto = Mapper.Map<ContractDto>(contractModel);

                _contractAppService.Create(contractDto, AuthHelper.GetCurrentUserId());

                return Ok("Contract Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateContract(ContractModel contractModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var contractDto = Mapper.Map<ContractDto>(contractModel);

                _contractAppService.Update(contractDto, AuthHelper.GetCurrentUserId());

                return Ok("Contract Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteContrat(long id)
        {
            try
            {
                _contractAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Contract Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
