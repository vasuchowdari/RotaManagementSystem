using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto;

namespace RMS.AppServiceLayer.UserDefinedSystemConfigs.Interfaces
{
    public interface IUserDefinedSystemConfigAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        ICollection<UserDefinedSystemConfigDto> GetAll();


        // CRUD
        void Update(UserDefinedSystemConfigDto udscDto, long userId);
    }
}
