using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Sites.Dto;

namespace RMS.AppServiceLayer.Sites.Interfaces
{
    public interface ISiteAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        SiteDto GetById(long id);
        ICollection<SiteDto> GetAll();
        ICollection<SiteDto> GetForCompany(long companyId); 

        // CRUD
        void Create(SiteDto siteDto, long userId);
        void Update(SiteDto siteDto, long userId);
        void Delete(long id, long userId);
    }
}
