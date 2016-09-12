using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Dto;

namespace RMS.AppServiceLayer.PersonnelLeaveProfiles.Interfaces
{
    public interface IPersonnelLeaveProfileAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        PersonnelLeaveProfileDto GetById(long id);
        ICollection<PersonnelLeaveProfileDto> GetAll();

        // CRUD
        void Create(PersonnelLeaveProfileDto personnelLeaveProfileDto, long userId);
        void Update(PersonnelLeaveProfileDto personnelLeaveProfileDto, long userId);
        void Delete(long id, long userId);
    }
}
