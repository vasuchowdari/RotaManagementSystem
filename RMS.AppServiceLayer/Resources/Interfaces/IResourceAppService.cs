using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Resources.Dto;

namespace RMS.AppServiceLayer.Resources.Interfaces
{
    public interface IResourceAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        ResourceDto GetById(long id);
        ICollection<ResourceDto> GetAll();

        // CRUD
        void Create(ResourceDto resourceDto, long userId);
        void Update(ResourceDto resourceDto, long userId);
        void Delete(long id, long userId);
    }
}
