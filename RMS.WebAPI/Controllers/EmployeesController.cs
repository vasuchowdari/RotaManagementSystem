using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Employees.Dto;
using RMS.AppServiceLayer.Employees.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeeAppService _employeeAppService;

        public EmployeesController(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var employeeDto = _employeeAppService.GetById(id);
                var employeeModel = Mapper.Map<EmployeeModel>(employeeDto);

                return Ok(employeeModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetByUserId/{UserId}")]
        public IHttpActionResult GetByUserId(long userId)
        {
            try
            {
                var employeeDto = _employeeAppService.GetByUserId(userId);

                if (employeeDto != null)
                {
                    var employeeModel = Mapper.Map<EmployeeModel>(employeeDto);

                    return Ok(employeeModel);
                }

                return Ok(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetForCompany/{CompanyId}")]
        public IHttpActionResult GetForCompany(long companyId)
        {
            try
            {
                var employeeDtos = _employeeAppService.GetForCompany(companyId);

                if (employeeDtos.Any())
                {
                    var employeeModels = new List<EmployeeModel>();

                    foreach (var employeeDto in employeeDtos)
                    {
                        employeeModels.Add(Mapper.Map<EmployeeModel>(employeeDto));
                    }

                    return Ok(employeeModels);    
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllForCompany/{CompanyId}")]
        public IHttpActionResult GetForAllCompany(long companyId)
        {
            try
            {
                var employeeDtos = _employeeAppService.GetAllForCompany(companyId);

                if (employeeDtos.Any())
                {
                    var employeeModels = new List<EmployeeModel>();

                    foreach (var employeeDto in employeeDtos)
                    {
                        employeeModels.Add(Mapper.Map<EmployeeModel>(employeeDto));
                    }

                    return Ok(employeeModels);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("CheckIfExists/{UserId}")]
        public IHttpActionResult CheckIfExists(long userId)
        {
            try
            {
                var employeeDto = _employeeAppService.GetByUserId(userId);

                if (employeeDto != null)
                {
                    return Ok(true);
                }

                return Ok(false);
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
                var employeeDtos = _employeeAppService.GetAll();
                var employeeModels = new List<EmployeeModel>();

                foreach (var employeeDto in employeeDtos)
                {
                    employeeModels.Add(Mapper.Map<EmployeeModel>(employeeDto));
                }

                return Ok(employeeModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetEmployeeNameAndId")]
        public IHttpActionResult GetEmployeeNameAndId()
        {
            try
            {
                var employeeNameIdList = new List<EmployeeNameIdModel>();
                var employeeNameIdDtos = _employeeAppService.GetEmployeeNameAndId();

                foreach (var employeeNameIdDto in employeeNameIdDtos)
                {
                    var employeeNameIdModel = new EmployeeNameIdModel
                    {
                        Id = employeeNameIdDto.Id,
                        Firstname = employeeNameIdDto.Firstname,
                        Lastname = employeeNameIdDto.Lastname
                    };

                    employeeNameIdList.Add(employeeNameIdModel);
                }

                return Ok(employeeNameIdList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetPersonalCalendarObjectsByDateRange")]
        public IHttpActionResult GetPersonalCalendarObjectsByDateRange(StaffCalendarObjectsByDateRangeModel model)
        {
            try
            {
                var staffCalendarDto = _employeeAppService.GetPersonalCalendarObjectsByDateRange(
                    model.EmployeeId, model.StartDate, model.EndDate);

                var staffCalendarModel = new StaffCalendarViewModel();
                staffCalendarModel.ShiftModels = new List<ShiftModel>();

                foreach (var shiftDto in staffCalendarDto.ShiftDtos)
                {
                    staffCalendarModel.ShiftModels.Add(Mapper.Map<ShiftModel>(shiftDto));
                }

                staffCalendarModel.LeaveRequestModels = new List<LeaveRequestModel>();
                foreach (var leaveRequestDto in staffCalendarDto.LeaveRequestDtos)
                {
                    staffCalendarModel.LeaveRequestModels.Add(Mapper.Map<LeaveRequestModel>(leaveRequestDto));
                }

                return Ok(staffCalendarModel);
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
        public IHttpActionResult CreateEmployee(EmployeeModel employeeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employeeDto = Mapper.Map<EmployeeDto>(employeeModel);

                _employeeAppService.Create(employeeDto, AuthHelper.GetCurrentUserId());

                return Ok("Employee Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateEmployee(EmployeeModel employeeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employeeDto = Mapper.Map<EmployeeDto>(employeeModel);

                _employeeAppService.Update(employeeDto, AuthHelper.GetCurrentUserId());

                return Ok("Employee Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteEmployee(long id)
        {
            try
            {
                _employeeAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Employee Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
