using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Budgets.Dto;
using RMS.AppServiceLayer.Budgets.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Budget")]
    public class BudgetsController : ApiController
    {
        private readonly IBudgetAppService _budgetAppService;

        public BudgetsController(IBudgetAppService budgetAppService)
        {
            _budgetAppService = budgetAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var budgetDto = _budgetAppService.GetById(id);
                var budgetModel = Mapper.Map<BudgetModel>(budgetDto);

                return Ok(budgetModel);
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
                var budgetDtos = _budgetAppService.GetAll();
                var budgetModels = new List<BudgetModel>();

                foreach (var budgetDto in budgetDtos)
                {
                    budgetModels.Add(Mapper.Map<BudgetModel>(budgetDto));
                }

                return Ok(budgetModels);
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
        public IHttpActionResult CreateBudget(BudgetModel budgetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var budgetDto = Mapper.Map<BudgetDto>(budgetModel);

                _budgetAppService.Create(budgetDto, AuthHelper.GetCurrentUserId());

                return Ok("Budget Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateBudget(BudgetModel budgetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var budgetDto = Mapper.Map<BudgetDto>(budgetModel);

                _budgetAppService.Update(budgetDto, AuthHelper.GetCurrentUserId());

                return Ok("Budget Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteBudget(long id)
        {
            try
            {
                _budgetAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Budget Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
