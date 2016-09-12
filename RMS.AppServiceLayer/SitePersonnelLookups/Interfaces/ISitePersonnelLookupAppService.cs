using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.SitePersonnelLookups.Dto;

namespace RMS.AppServiceLayer.SitePersonnelLookups.Interfaces
{
    public interface ISitePersonnelLookupAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        SitePersonnelLookupDto GetById(long id);
        ICollection<SitePersonnelLookupDto> GetAll();
        ICollection<SitePersonnelLookupDto> GetForIds(long? employeeId);
        ICollection<SitePersonnelLookupDto> GetAllForEmployee(long employeeId);
        ICollection<SitePersonnelLookupDto> GetAllForSite(long siteId);
        ICollection<SitePersonnelLookupDto> GetAllPlusInactiveForEmployee(long employeeId);



        // CRUD
        void Create(SitePersonnelLookupDto sitePersonnelLookupDto, long userId);
        void Update(SitePersonnelLookupDto sitePersonnelLookupDto, long userId);
        void Delete(long id, long userId);
    }
}
