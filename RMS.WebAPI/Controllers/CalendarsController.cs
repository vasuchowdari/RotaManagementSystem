using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RMS.AppServiceLayer.Calendars.Dto;
using RMS.AppServiceLayer.Calendars.Interfaces;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Calendar")]
    public class CalendarsController : ApiController
    {
        private readonly ICalendarAppService _calendarAppService;

        public CalendarsController(ICalendarAppService calendarAppService)
        {
            _calendarAppService = calendarAppService;
        }


        // Repo Actions
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var calendarDto = _calendarAppService.GetById(id);
                var calendarModel = Mapper.Map<CalendarModel>(calendarDto);

                return Ok(calendarModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Site/{SiteId}")]
        public IHttpActionResult GetForSite(long siteId)
        {
            try
            {
                var calendarDtos = _calendarAppService.GetForSite(siteId);

                if (calendarDtos != null)
                {
                    var calendarModels = new List<CalendarModel>();

                    foreach (var calendarDto in calendarDtos)
                    {
                        calendarModels.Add(Mapper.Map<CalendarModel>(calendarDto));
                    }

                    return Ok(calendarModels);
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
        [Route("SubSite/{SubSiteId}")]
        public IHttpActionResult GetForSubSite(long subSiteId)
        {
            try
            {
                var calendarDtos = _calendarAppService.GetForSubSite(subSiteId);

                if (calendarDtos != null)
                {
                    var calendarModels = new List<CalendarModel>();

                    foreach (var calendarDto in calendarDtos)
                    {
                        calendarModels.Add(Mapper.Map<CalendarModel>(calendarDto));
                    }

                    return Ok(calendarModels);
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
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var calendarDtos = _calendarAppService.GetAll();
                var calendarModels = new List<CalendarModel>();

                foreach (var calendarDto in calendarDtos)
                {
                    calendarModels.Add(Mapper.Map<CalendarModel>(calendarDto));
                }

                return Ok(calendarModels);
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
        public IHttpActionResult CreateCalendar(CalendarModel calendarModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var calendarDto = Mapper.Map<CalendarDto>(calendarModel);

                _calendarAppService.Create(calendarDto, AuthHelper.GetCurrentUserId());

                return Ok("Calendar Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateCalendar(CalendarModel calendarModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var calendarDto = Mapper.Map<CalendarDto>(calendarModel);

                _calendarAppService.Update(calendarDto, AuthHelper.GetCurrentUserId());

                return Ok("Calendar Update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteCalendar(long id)
        {
            try
            {
                _calendarAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("Calendar Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
