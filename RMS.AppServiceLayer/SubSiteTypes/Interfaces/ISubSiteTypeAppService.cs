using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.SubSiteTypes.Dto;

namespace RMS.AppServiceLayer.SubSiteTypes.Interfaces
{
    public interface ISubSiteTypeAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        SubSiteTypeDto GetById(long id);
        ICollection<SubSiteTypeDto> GetAll();

        // CRUD
        void Create(SubSiteTypeDto subSiteTypeDto, long userId);
        void Update(SubSiteTypeDto subSiteTypeDto, long userId);
        void Delete(long id, long userId);
    }
}
