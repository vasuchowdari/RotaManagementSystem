using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.Accounts.Interfaces;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Helpers.Dto;
using RMS.AppServiceLayer.Helpers.Services;
using RMS.AppServiceLayer.Users.Dto;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Accounts.Services
{
    public class AccountAppService : IAccountAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public AccountAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }


        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public bool CheckIfEmailExists(string email)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == email).FirstOrDefault();

            if (user != null)
            {
                return true;
            }

            return false;
        }

        public bool CheckIfLoginExists(string login)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Login == login).FirstOrDefault();

            if (user != null)
            {
                return true;
            }

            return false;
        }

        public bool CheckIfExternalTimeSystemIdExists(int extTimeSysId)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ExternalTimeSystemId == extTimeSysId).FirstOrDefault();

            if (user != null)
            {
                return true;
            }

            return false;
        }


        // Repo Methods
        public long GetLoggedInUserId(string login)
        {
            var userId = _unitOfWork.UserRepository.Get(u => u.Login == login)
                                                   .Select(u => u.Id)
                                                   .FirstOrDefault();

            return userId;
        }

        public bool Login(string login, string password)
        {
            // validate password
            var hashToCheck = CommonHelperAppService.HashPassword(password);

            return CommonHelperAppService.ValidatePassword(password, hashToCheck);
        }


        // CRUD
        public long RegisterUserAccount(UserDto userDto, long userId)
        {
            // gen random string and hash
            var unhashedPassword = CommonHelperAppService.RandomString(8);
            userDto.Password = unhashedPassword;

            var user = Mapper.Map<User>(userDto);
            user.Password = HashPassword(unhashedPassword);

            _unitOfWork.UserRepository.Create(user);
            _unitOfWork.Save();


            // Mail
            MailerService.SendUserRegisteredEmail(userDto);

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.UserTableName,
                userId,
                user.Id);

            return user.Id;
        }

        // TODO: Move this to UserAppService?
        public void UpdateUserPassword(UserDto userDto, long userId)
        {
            userDto.Password = HashPassword(userDto.Password);
            var user = Mapper.Map<User>(userDto);

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


        // Private Methods
        private static string HashPassword(string password)
        {
            var saltedHash = CommonHelperAppService.HashPassword(password);

            return saltedHash;
        }
    }
}
