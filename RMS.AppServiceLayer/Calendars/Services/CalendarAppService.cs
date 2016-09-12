using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Calendars.Dto;
using RMS.AppServiceLayer.Calendars.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Calendars.Services
{
    public class CalendarAppService : ICalendarAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public CalendarAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }


        // Repo Methods
        public CalendarDto GetById(long id)
        {
            var calendar = _unitOfWork.CalendarRepository.GetById(id);

            if (calendar != null)
            {
                var calendarDto = Mapper.Map<CalendarDto>(calendar);

                return calendarDto;
            }

            return null;
        }

        public ICollection<CalendarDto> GetAll()
        {
            var calendars = _unitOfWork.CalendarRepository.GetAll();

            if (calendars != null)
            {
                var calendarDtos = new List<CalendarDto>();

                foreach (var calendar in calendars)
                {
                    calendarDtos.Add(Mapper.Map<CalendarDto>(calendar));
                }

                return calendarDtos;
            }

            return null;
        }

        public ICollection<CalendarDto> GetForSite(long siteId)
        {
            var calendars = _unitOfWork.CalendarRepository.Get(
                    c => c.SiteId == siteId && c.SubSiteId == null && c.IsActive,
                    null,
                    "CalendarResourceRequirements");

            if (calendars != null)
            {
                var calendarDtos = new List<CalendarDto>();

                foreach (var calendar in calendars)
                {
                    calendarDtos.Add(Mapper.Map<CalendarDto>(calendar));
                }

                return calendarDtos;
            }

            return null;
        }

        public ICollection<CalendarDto> GetForSubSite(long subSiteId)
        {
            var calendars = _unitOfWork.CalendarRepository.Get(
                c => c.SubSiteId == subSiteId && c.IsActive,
                null,
                "CalendarResourceRequirements");

            if (calendars != null)
            {
                var calendarDtos = new List<CalendarDto>();

                foreach (var calendar in calendars)
                {
                    calendarDtos.Add(Mapper.Map<CalendarDto>(calendar));
                }

                return calendarDtos;
            }

            return null;
        }


        // CRUD
        public void Create(CalendarDto calendarDto, long userId)
        {
            var calendar = Mapper.Map<Calendar>(calendarDto);

            _unitOfWork.CalendarRepository.Create(calendar);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.CalendarTableName,
                userId,
                calendar.Id);
        }

        public void Update(CalendarDto calendarDto, long userId)
        {
            var calendar = _unitOfWork.CalendarRepository.GetById(calendarDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(calendarDto, calendar);

            _unitOfWork.CalendarRepository.Update(calendar);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.CalendarTableName,
                userId,
                calendar.Id);
        }

        public void Delete(long id, long userId)
        {
            var calendar = _unitOfWork.CalendarRepository.GetById(id);

            calendar.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.CalendarRepository.Update(calendar);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.CalendarTableName,
                userId,
                calendar.Id);
        }
    }
}
