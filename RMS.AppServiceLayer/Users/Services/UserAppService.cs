using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Helpers.Services;
using RMS.AppServiceLayer.Users.Dto;
using RMS.AppServiceLayer.Users.Interfaces;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Users.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public UserAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public string HashPassword(string password)
        {
            var saltedHash = CommonHelperAppService.HashPassword(password);

            return saltedHash;
        }

        public bool IsAccountLocked(string userLogin)
        {
            var isUserActive = _unitOfWork.UserRepository.Get(u => u.Login == userLogin &&
                                                           u.IsActive)
                                                         .Select(u => u.IsAccountLocked)
                                                         .FirstOrDefault();

            return isUserActive;
        }

        public UserDto VerifyUser(string userLogin)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Login == userLogin && u.IsActive,
                null,
                "SystemAccessRole")
                .FirstOrDefault();

            if (user != null)
            {
                var userDto = Mapper.Map<UserDto>(user);

                return userDto;
            }

            return null;
        }

        public bool VerifyPassword(string enteredPassword, string retrievedPassword)
        {
            var isValid = CommonHelperAppService.ValidatePassword(enteredPassword, retrievedPassword);

            if (!isValid)
            {
                return false;
            }

            return true;
        }

        public bool PasswordReset(string emailAddress)
        {
            // get user based upon email address
            var user = _unitOfWork.UserRepository.Get(u => u.Email == emailAddress).FirstOrDefault();

            if (user != null)
            {
                // for use below with unhashed password
                var userDto = Mapper.Map<UserDto>(user);

                // generate new password from Random
                userDto.Password = CommonHelperAppService.RandomString(8);

                // Hash it. Hash it, real good!
                user.Password = CommonHelperAppService.HashPassword(userDto.Password);

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();

                MailerService.SendPasswordResetEmail(userDto);

                return true;
            }

            return false;
        }


        // Repo Methods
        public UserDto GetById(long id)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Id == id, null, "SystemAccessRole")
                                                 .FirstOrDefault();

            if (user != null)
            {
                var userDto = Mapper.Map<UserDto>(user);

                return userDto;
            }

            return null;
        }

        public ICollection<UserDto> GetAll()
        {
            var users = _unitOfWork.UserRepository.GetAll();

            if (users != null)
            {
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    userDtos.Add(Mapper.Map<UserDto>(user));
                }

                return userDtos;
            }

            return null;
        }

        public ICollection<UserDto> GetAllActive()
        {
            var users = _unitOfWork.UserRepository.Get(u => u.IsActive)
                                                  .ToList();

            if (users != null)
            {
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    userDtos.Add(Mapper.Map<UserDto>(user));
                }

                return userDtos;
            }

            return null;
        }

        public ICollection<UserDto> GetAllInactive()
        {
            var users = _unitOfWork.UserRepository.Get(u => u.IsActive == false)
                                                  .ToList();

            if (users != null)
            {
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    userDtos.Add(Mapper.Map<UserDto>(user));
                }

                return userDtos;
            }

            return null;
        }

        public ICollection<UserListItemDto> GetUsernameAndId()
        {
            var userListItems = _unitOfWork.UserRepository.GetAll()
                                                          .Select(u => new UserListItemDto
                                                          {
                                                              Id = u.Id,
                                                              Firstname = u.Firstname,
                                                              Lastname = u.Lastname
                                                          })
                                                          .OrderBy(u => u.Firstname)
                                                          .ToList();

            return userListItems;
        }


        // CRUD
        public void Update(UserDto userDto, long userId)
        {
            var user = _unitOfWork.UserRepository.GetById(userDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(userDto, user);

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.UserTableName,
                userId,
                user.Id);
        }

        public void Delete(long id, long userId)
        {
            var user = _unitOfWork.UserRepository.GetById(id);

            user.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.UserTableName,
                userId,
                user.Id);
        }


        // Private Methods
        
    }
}