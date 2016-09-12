using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.BudgetPeriods.Dto;
using RMS.AppServiceLayer.BudgetPeriods.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/BudgetPeriod")]
    public class BudgetPeriodsController : ApiController
    {
        private readonly IBudgetPeriodAppService _budgetPeriodAppService;

        public BudgetPeriodsController(IBudgetPeriodAppService budgetPeriodAppService)
        {
            _budgetPeriodAppService = budgetPeriodAppService;
        }


        // Repo Methods
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var budgetPeriodDto = _budgetPeriodAppService.GetById(id);
                var budgetPeriodModel = Mapper.Map<BudgetPeriodModel>(budgetPeriodDto);

                return Ok(budgetPeriodModel);
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
                var budgetPeriodDtos = _budgetPeriodAppService.GetAll();
                var budgetPeriodModels = new List<BudgetPeriodModel>();

                foreach (var budgetPeriodDto in budgetPeriodDtos)
                {
                    budgetPeriodModels.Add(Mapper.Map<BudgetPeriodModel>(budgetPeriodDto));
                }

                return Ok(budgetPeriodModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // CRUD Methods
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult CreateBudgetPeriod(BudgetPeriodModel budgetPeriodModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var budgetPeriodDto = Mapper.Map<BudgetPeriodDto>(budgetPeriodModel);

                _budgetPeriodAppService.Create(budgetPeriodDto, AuthHelper.GetCurrentUserId());

                return Ok("Budget Period Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateBudgetPeriod(BudgetPeriodModel budgetPeriodModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var budgetPeriodDto = Mapper.Map<BudgetPeriodDto>(budgetPeriodModel);

                _budgetPeriodAppService.Update(budgetPeriodDto, AuthHelper.GetCurrentUserId());

                return Ok("Budget Period Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteBudgetPeriod(long id)
        {
            try
            {
                _budgetPeriodAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Budget Period Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
