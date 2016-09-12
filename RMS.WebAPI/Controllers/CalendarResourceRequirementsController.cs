using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.CalendarResourceRequirements.Dto;
using RMS.AppServiceLayer.CalendarResourceRequirements.Interfaces;
using RMS.AppServiceLayer.Shifts.Dto;
using RMS.AppServiceLayer.Shifts.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/ResourceRequirement")]
    public class 
        CalendarResourceRequirementsController : ApiController
    {
        private readonly ICalResourceRqAppService _calResourceRqAppService;
        private readonly IShiftAppService _shiftAppService;

        public CalendarResourceRequirementsController(
            ICalResourceRqAppService calResourceRqAppService,
            IShiftAppService shiftAppService)
        {
            _calResourceRqAppService = calResourceRqAppService;
            _shiftAppService = shiftAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var calResRqDto = _calResourceRqAppService.GetById(id);
                var calResRqModel = Mapper.Map<CalendarResourceRequirementModel>(calResRqDto);

                return Ok(calResRqModel);
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
                var calResRqDtos = _calResourceRqAppService.GetAll();
                var calResRqModels = new List<CalendarResourceRequirementModel>();

                foreach (var calResRqDto in calResRqDtos)
                {
                    calResRqModels.Add(Mapper.Map<CalendarResourceRequirementModel>(calResRqDto));
                }

                return Ok(calResRqModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Calendar/{CalendarId}/{StartDate}/{EndDate}")]
        public IHttpActionResult GetForCalendar(long calendarId, DateTime startDate, DateTime endDate)
        {
            // sets to 23:59:59 - DO NOT DELETE!
            endDate = endDate.AddSeconds(-1);

            var calResRqDtos = _calResourceRqAppService.GetForCalendar(calendarId, startDate, endDate);
            var managerAreaRotaModelList = new List<ManagerAreaRotaModel>();

            foreach (var calResRqDto in calResRqDtos)
            {
                var managerAreaRotaModel = new ManagerAreaRotaModel();
                var resRqModel = Mapper.Map<CalendarResourceRequirementModel>(calResRqDto);

                var shiftModels = new List<ShiftModel>();
                var shiftDtos = _shiftAppService.GetForCalendarResourceRequirment(calResRqDto.Id, startDate, endDate);
                foreach (var shiftDto in shiftDtos)
                {
                    shiftModels.Add(Mapper.Map<ShiftModel>(shiftDto));
                }

                resRqModel.ShiftModels = shiftModels;
                
                managerAreaRotaModel.CalendarResourceRequirementModel = resRqModel;
                managerAreaRotaModelList.Add(managerAreaRotaModel);
            }

            return Ok(managerAreaRotaModelList);
        }


        // CRUD Actions
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult CreateShifts(ICollection<ManagerAreaRotaModel> rotaModels)
        {
            try
            {
                var calResRqIdList = new List<long>();
                foreach (var rotaModel in rotaModels)
                {
                    // CPRR ######################################
                    var calResRqDto =
                        Mapper.Map<CalendarResourceRequirementDto>(rotaModel.CalendarResourceRequirementModel);
                    
                    var calResRqId = _calResourceRqAppService.Create(calResRqDto, AuthHelper.GetCurrentUserId());
                    calResRqIdList.Add(calResRqId);

                    // Shifts (and Personnel) ####################
                    foreach (var shiftModel in rotaModel.CalendarResourceRequirementModel.ShiftModels)
                    {
                        shiftModel.CalendarResourceRequirementId = calResRqId;

                        var shiftDto = Mapper.Map<ShiftDto>(shiftModel);
                        if (_shiftAppService.CheckIfShiftExistsForResourceReq(shiftDto))
                        {
                            return BadRequest("Shift already exists.");
                        };

                        _shiftAppService.Create(shiftDto, AuthHelper.GetCurrentUserId());
                    }
                }

                return Ok(calResRqIdList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // THIS IS A POST RATHER THAN PUT AS NOUVITA WEB SERVER WAS
        // DENYING ACCESS WITH PUT COMMAND. POST WORKED, SO POST IT IS
        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateShifts(ICollection<ManagerAreaRotaModel> rotaModels)
        {
            try
            {
                foreach (var rotaModel in rotaModels)
                {
                    var calResRqDto = 
                        Mapper.Map<CalendarResourceRequirementDto>(rotaModel.CalendarResourceRequirementModel);
                    
                    _calResourceRqAppService.Update(calResRqDto, AuthHelper.GetCurrentUserId());

                    // Shifts (and Personnel) ####################
                    foreach (var shiftModel in rotaModel.CalendarResourceRequirementModel.ShiftModels)
                    {
                        shiftModel.CalendarResourceRequirementId = calResRqDto.Id;

                        var shiftDto = Mapper.Map<ShiftDto>(shiftModel);
                        if (_shiftAppService.CheckIfShiftExistsForResourceReq(shiftDto))
                        {
                            return BadRequest("Shift already exists.");
                        };

                        _shiftAppService.Create(shiftDto, AuthHelper.GetCurrentUserId());
                    }
                }
                
                return Ok("The rota has been updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteShift(long id)
        {
            try
            {
                _calResourceRqAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Resource Requirement Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
