using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.CalendarResourceRequirements.Dto;
using RMS.AppServiceLayer.CalendarResourceRequirements.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.CalendarResourceRequirements.Services
{
    public class CalResourceRqAppService : ICalResourceRqAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public CalResourceRqAppService(IAuditLogAppService auditLogAppService)
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
        public CalendarResourceRequirementDto GetById(long id)
        {
            var calResRq = _unitOfWork.CalendarResourceRequirementRepository.GetById(id);

            if (calResRq != null)
            {
                var calResRqDto = Mapper.Map<CalendarResourceRequirementDto>(calResRq);

                return calResRqDto;
            }

            return null;
        }

        public ICollection<CalendarResourceRequirementDto> GetAll()
        {
            var calResRqs = _unitOfWork.CalendarResourceRequirementRepository.GetAll();

            if (calResRqs != null)
            {
                var calResRqDtos = new List<CalendarResourceRequirementDto>();

                foreach (var calResRq in calResRqs)
                {
                    calResRqDtos.Add(Mapper.Map<CalendarResourceRequirementDto>(calResRq));
                }

                return calResRqDtos;
            }

            return null;
        }

        public ICollection<CalendarResourceRequirementDto> GetForCalendar(long calendarId, DateTime startDate, DateTime endDate)
        {
            var calResRqs = _unitOfWork.CalendarResourceRequirementRepository.Get(
                                        crr => crr.CalendarId == calendarId && crr.IsActive &&
                                        crr.StartDate <= endDate &&
                                        crr.EndDate >= startDate);

            if (calResRqs != null)
            {
                var calResRqDtos = new List<CalendarResourceRequirementDto>();

                foreach (var calResRq in calResRqs)
                {
                    calResRqDtos.Add(Mapper.Map<CalendarResourceRequirementDto>(calResRq));
                }

                return calResRqDtos;
            }

            return null;
        }


        // CRUD
        public long Create(CalendarResourceRequirementDto calendarResourceRequirementDto, long userId)
        {
            var calResRq = Mapper.Map<CalendarResourceRequirement>(calendarResourceRequirementDto);

            _unitOfWork.CalendarResourceRequirementRepository.Create(calResRq);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.CalendarResourceRequirementTableName,
                userId,
                calResRq.Id);

            return calResRq.Id;
        }

        public void Update(CalendarResourceRequirementDto calendarResourceRequirementDto, long userId)
        {
            var calResRq =
                _unitOfWork.CalendarResourceRequirementRepository.GetById(calendarResourceRequirementDto.Id);



            if (calResRq.StartDate < calendarResourceRequirementDto.StartDate)
            {
                calendarResourceRequirementDto.StartDate = calResRq.StartDate;
            }


            if (calResRq.EndDate > calendarResourceRequirementDto.EndDate)
            {
                // keep existing start date for existing cal res req lines.
                calendarResourceRequirementDto.EndDate = calResRq.EndDate;
            }

            CommonHelperAppService.MapDtoToEntityForUpdating(calendarResourceRequirementDto, calResRq);

            _unitOfWork.CalendarResourceRequirementRepository.Update(calResRq);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.CalendarResourceRequirementTableName,
                userId,
                calResRq.Id);
        }

        public void Delete(long id, long userId)
        {
            var shifts = _unitOfWork.ShiftRepository.Get(s => s.IsActive &&
                                                              s.CalendarResourceRequirementId == id)
                .ToList();

            var historicalShifts = false;
            foreach (var shift in shifts)
            {
                if (shift.StartDate >= DateTime.Now)
                {
                    _unitOfWork.ShiftRepository.Delete(shift.Id);

                    // Audit
                    _auditLogAppService.Audit(
                        AppConstants.ActionTypeDelete,
                        AppConstants.ShiftTableName,
                        userId,
                        shift.Id);
                }
                else
                {
                    historicalShifts = true;
                }
            }
            _unitOfWork.Save();

            // no historical shifts, safe to delete entire cal res requirement 
            if (!historicalShifts)
            {
                var calResRq = _unitOfWork.CalendarResourceRequirementRepository.GetById(id);

                _unitOfWork.CalendarResourceRequirementRepository.Delete(calResRq);
                _unitOfWork.Save();

                // Audit
                _auditLogAppService.Audit(
                    AppConstants.ActionTypeDelete,
                    AppConstants.CalendarResourceRequirementTableName,
                    userId,
                    calResRq.Id);
            }
            else
            {
                // roll End Date to 23:59:59 of closest future saturday
                var calResRq = _unitOfWork.CalendarResourceRequirementRepository.GetById(id);

                var today = DateTime.Today;
                // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
                var daysUntilSatrurday = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
                var nextSaturday = today.AddDays(daysUntilSatrurday);

                calResRq.EndDate = nextSaturday.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                _unitOfWork.CalendarResourceRequirementRepository.Update(calResRq);
                _unitOfWork.Save();

                // Audit
                _auditLogAppService.Audit(
                    AppConstants.ActionTypeUpdate,
                    AppConstants.CalendarResourceRequirementTableName,
                    userId,
                    calResRq.Id);
            }
        }
    }
}
