using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Users.Dto;

namespace RMS.AppServiceLayer.Users.Interfaces
{
    public interface IUserAppService : IDisposable
    {
        // Service Methods
        string HashPassword(string password);
        bool PasswordReset(string emailAddress);
        bool VerifyPassword(string enteredPassword, string retrievedPassword);
        bool IsAccountLocked(string userLogin);
        UserDto VerifyUser(string userLogin);


        // Repo Methods
        UserDto GetById(long id);
        ICollection<UserDto> GetAll();
        ICollection<UserDto> GetAllActive();
        ICollection<UserDto> GetAllInactive();
        ICollection<UserListItemDto> GetUsernameAndId();


        // CRUD
        void Update(UserDto userDto, long userId);
        void Delete(long id, long userId);
    }
}
