using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.ResourceRateModifiers.Dto;

namespace RMS.AppServiceLayer.ResourceRateModifiers.Interfaces
{
    public interface IResourceRateModifierAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        ResourceRateModifierDto GetById(long id);
        ICollection<ResourceRateModifierDto> GetAll();

        // CRUD
        void Create(ResourceRateModifierDto rateModifierDto, long userId);
        void Update(ResourceRateModifierDto rateModifierDto, long userId);
        void Delete(long id, long userId);
    }
}
