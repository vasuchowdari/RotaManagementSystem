using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Genders.Dto;

namespace RMS.AppServiceLayer.Genders.Interfaces
{
    public interface IGenderAppService : IDisposable
    {
        // Servie Methods


        // Repo Methods
        GenderDto GetById(long id);
        ICollection<GenderDto> GetAll();

        // CRUD
        void Create(GenderDto genderDto, long userId);
        void Update(GenderDto genderDto, long userId);
        void Delete(long id, long userId);
    }
}
