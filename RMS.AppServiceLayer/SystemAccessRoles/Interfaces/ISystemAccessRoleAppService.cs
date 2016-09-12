using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.SystemAccessRoles.Dto;

namespace RMS.AppServiceLayer.SystemAccessRoles.Interfaces
{
    public interface ISystemAccessRoleAppService : IDisposable
    {
        // Service Methods

        // Repo Methods
        ICollection<SystemAccessRoleDto> GetAll();
        SystemAccessRoleDto GetById(long id);

        // CRUD
        void Create(SystemAccessRoleDto systemAccessRoleDto, long userId);
        void Update(SystemAccessRoleDto systemAccessRoleDto, long userId);
        void Delete(long id, long userId);
    }
}
