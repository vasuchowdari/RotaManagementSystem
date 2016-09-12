using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.SiteTypes.Dto;

namespace RMS.AppServiceLayer.SiteTypes.Interfaces
{
    public interface ISiteTypeAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        SiteTypeDto GetById(long id);
        ICollection<SiteTypeDto> GetAll();

        // CRUD
        void Create(SiteTypeDto siteTypeDto, long userId);
        void Update(SiteTypeDto siteTypeDto, long userId);
        void Delete(long id, long userId);
    }
}
