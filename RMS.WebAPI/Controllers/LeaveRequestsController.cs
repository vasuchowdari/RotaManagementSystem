using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Employees.Interfaces;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.LeaveRequests.Interfaces;
using RMS.AppServiceLayer.Users.Interfaces;
using RMS.AppServiceLayer.Zktime.Enums;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/LeaveRequest")]
    public class LeaveRequestsController : ApiController
    {
        private readonly ILeaveRequestAppService _leaveRequestAppService;
        private readonly IUserAppService _userAppService;
        private readonly IEmployeeAppService _employeeAppService;

        public LeaveRequestsController(
            ILeaveRequestAppService leaveRequestAppService, 
            IUserAppService userAppService,
            IEmployeeAppService employeeAppService)
        {
            _leaveRequestAppService = leaveRequestAppService;
            _userAppService = userAppService;
            _employeeAppService = employeeAppService;
        }


        // Service Actions
        [Authorize]
        [HttpGet]
        [Route("GetForEmployee/{Id}")]
        public IHttpActionResult GetForEmployee(long id)
        {
            try
            {
                var leaveRequestDtos = _leaveRequestAppService.GetForEmployee(id);
                var leaveRequestModels = new List<LeaveRequestModel>();

                foreach (var leaveRequestDto in leaveRequestDtos)
                {
                    leaveRequestModels.Add(Mapper.Map<LeaveRequestModel>(leaveRequestDto));
                }

                return Ok(leaveRequestModels);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var leaveRequestDto = _leaveRequestAppService.GetById(id);
                var leaveRequestModel = Mapper.Map<LeaveRequestModel>(leaveRequestDto);

                return Ok(leaveRequestModel);
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
                var leaveRequestDtos = _leaveRequestAppService.GetAll();
                var leaveRequestModels = new List<LeaveRequestModel>();

                foreach (var leaveRequestDto in leaveRequestDtos)
                {
                    leaveRequestModels.Add(Mapper.Map<LeaveRequestModel>(leaveRequestDto));
                }

                return Ok(leaveRequestModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetForIds")]
        public IHttpActionResult GetForIds(List<long> staffIds)
        {
            try
            {
                var leaveRequestDtos = _leaveRequestAppService.GetForIds(staffIds);
                var leaveRequestModels = new List<LeaveRequestModel>();

                foreach (var leaveRequestDto in leaveRequestDtos)
                {
                    leaveRequestModels.Add(Mapper.Map<LeaveRequestModel>(leaveRequestDto));
                }

                foreach (var leaveRequestModel in leaveRequestModels)
                {
                    if (leaveRequestModel.EmployeeId != null)
                    {
                        var employeeDto = _employeeAppService.GetById((long)leaveRequestModel.EmployeeId);
                        var user = _userAppService.GetById(employeeDto.UserId);

                        leaveRequestModel.StaffName = user.Firstname + " " + user.Lastname;    
                    }
                }

                return Ok(leaveRequestModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // CRUD Actions
        [Authorize]
        [HttpPost]
        [Route("Admin/Create")]
        public IHttpActionResult AdminCreateLeaveRequest(LeaveRequestModel leaveRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaveRequestDto = Mapper.Map<LeaveRequestDto>(leaveRequestModel);

                // TEMP
                leaveRequestDto.ActualStartDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                leaveRequestDto.ActualEndDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                leaveRequestDto.ZktStartDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                leaveRequestDto.ZktEndDateTime = new DateTime(1900, 1, 1, 0, 0, 0);

                if (leaveRequestDto.LeaveTypeId == 7)
                {
                    leaveRequestDto.Status = (int)ShiftProfileStatus.Training; // training and zkt needed
                }
                // TEMP

                var result = _leaveRequestAppService.Create(leaveRequestDto, AuthHelper.GetCurrentUserId());

                // Shift
                if (result == 0)
                {
                    return BadRequest("Staff already on Shift.");
                }

                // Anunal
                if (result == 1)
                {
                    return BadRequest("Staff already on Annual Leave.");
                }

                // Maternity
                if (result == 2)
                {
                    return BadRequest("Staff already on Maternity.");
                }

                // Paternity
                if (result == 3)
                {
                    return BadRequest("Staff already on Paternity.");
                }

                // Sick
                if (result == 4)
                {
                    return BadRequest("Staff already on Sick.");
                }

                // Suspension
                if (result == 5)
                {
                    return BadRequest("Staff already on Suspension.");
                }

                // Other
                if (result == 6)
                {
                    return BadRequest("Staff already on Other Leave.");
                }

                // Training
                if (result == 7)
                {
                    return BadRequest("Staff already on Training.");
                }

                return Ok("Staff Available.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Admin/Update")]
        public IHttpActionResult AdminUpdateLeaveRequest(LeaveRequestModel leaveRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaveRequestDto = Mapper.Map<LeaveRequestDto>(leaveRequestModel);

                var result = _leaveRequestAppService.Update(leaveRequestDto, AuthHelper.GetCurrentUserId());

                // Shift
                if (result == 0)
                {
                    return BadRequest("Staff already on Shift.");
                }

                // Anunal
                if (result == 1)
                {
                    return BadRequest("Staff already on Annual Leave.");
                }

                // Maternity
                if (result == 2)
                {
                    return BadRequest("Staff already on Maternity.");
                }

                // Paternity
                if (result == 3)
                {
                    return BadRequest("Staff already on Paternity.");
                }

                // Sick
                if (result == 4)
                {
                    return BadRequest("Staff already on Sick.");
                }

                // Suspension
                if (result == 5)
                {
                    return BadRequest("Staff already on Suspension.");
                }

                // Other
                if (result == 6)
                {
                    return BadRequest("Staff already on Other Leave.");
                }

                // Training
                if (result == 7)
                {
                    return BadRequest("Staff already on Training.");
                }

                return Ok("Leave Request Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteLeaveRequest(long id)
        {
            try
            {
                _leaveRequestAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Leave Request Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
