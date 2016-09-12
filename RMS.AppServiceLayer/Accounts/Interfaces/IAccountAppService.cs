using System;
using RMS.AppServiceLayer.Users.Dto;

namespace RMS.AppServiceLayer.Accounts.Interfaces
{
    public interface IAccountAppService : IDisposable
    {
        // service
        bool CheckIfEmailExists(string email);
        bool CheckIfLoginExists(string login);
        bool CheckIfExternalTimeSystemIdExists(int extTimeSysId);

        // Repo
        long GetLoggedInUserId(string login);
        bool Login(string login, string password);

        // CRUD
        long RegisterUserAccount(UserDto userDto, long userId);
        void UpdateUserPassword(UserDto userDto, long userId);
    }
}
