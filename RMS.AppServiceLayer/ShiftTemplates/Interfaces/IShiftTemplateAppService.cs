using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.ShiftTemplates.Dto;

namespace RMS.AppServiceLayer.ShiftTemplates.Interfaces
{
    public interface IShiftTemplateAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        ShiftTemplateDto GetById(long id);
        ICollection<ShiftTemplateDto> GetAll();
        ICollection<ShiftTemplateDto> GetForSite(long siteId);
        //ICollection<ShiftTemplateDto> GetForSubSite(long subsiteId);
        ICollection<ShiftTemplateDto> GetBySearchCriteria(long resourceId, long siteId, long? subsiteId);

        // CRUD
        void Create(ShiftTemplateDto shiftTemplateDto, long userId);
        void Update(ShiftTemplateDto shiftTemplateDto, long userId);
        void Delete(long id, long userId);
    }
}
