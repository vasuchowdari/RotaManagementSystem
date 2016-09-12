using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Calendars.Dto;

namespace RMS.AppServiceLayer.Calendars.Interfaces
{
    public interface ICalendarAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        CalendarDto GetById(long id);
        ICollection<CalendarDto> GetAll();
        ICollection<CalendarDto> GetForSite(long siteId);
        ICollection<CalendarDto> GetForSubSite(long subSiteId); 

        // CRUD
        void Create(CalendarDto calendarDto, long userId);
        void Update(CalendarDto calendarDto, long userId);
        void Delete(long id, long userId);
    }
}
