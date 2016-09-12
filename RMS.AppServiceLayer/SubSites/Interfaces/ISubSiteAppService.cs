using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.SubSites.Dto;

namespace RMS.AppServiceLayer.SubSites.Interfaces
{
    public interface ISubSiteAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        SubSiteDto GetById(long id);
        ICollection<SubSiteDto> GetAll();

        // CRUD
        void Create(SubSiteDto subSiteDto, long userId);
        void Update(SubSiteDto subSiteDto, long userId);
        void Delete(long id, long userId);
    }
}
