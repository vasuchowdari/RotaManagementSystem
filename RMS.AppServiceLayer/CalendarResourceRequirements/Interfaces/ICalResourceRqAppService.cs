using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.CalendarResourceRequirements.Dto;

namespace RMS.AppServiceLayer.CalendarResourceRequirements.Interfaces
{
    public interface ICalResourceRqAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        CalendarResourceRequirementDto GetById(long id);
        ICollection<CalendarResourceRequirementDto> GetAll();
        ICollection<CalendarResourceRequirementDto> GetForCalendar(long calendarId, DateTime startDate, DateTime endDate); 


        // CRUD
        long Create(CalendarResourceRequirementDto calendarResourceRequirementDto, long userId);
        void Update(CalendarResourceRequirementDto calendarResourceRequirementDto, long userId);
        void Delete(long id, long userId);
    }
}
