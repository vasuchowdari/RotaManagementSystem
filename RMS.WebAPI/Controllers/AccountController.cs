using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;
using AutoMapper;
using RMS.AppServiceLayer.Accounts.Interfaces;
using RMS.AppServiceLayer.Contracts.Dto;
using RMS.AppServiceLayer.Contracts.Interfaces;
using RMS.AppServiceLayer.Employees.Dto;
using RMS.AppServiceLayer.Employees.Interfaces;
using RMS.AppServiceLayer.SitePersonnelLookups.Dto;
using RMS.AppServiceLayer.SitePersonnelLookups.Interfaces;
using RMS.AppServiceLayer.Users.Dto;
using RMS.AppServiceLayer.Users.Interfaces;
using RMS.AppServiceLayer.Zktime;
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IAccountAppService _accountAppService;
        private readonly IUserAppService _userAppService;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IContractAppService _contractAppService;
        private readonly ISitePersonnelLookupAppService _sitePersonnelLookupAppService;

        public AccountController(IAccountAppService accountAppService, IUserAppService userAppService, IEmployeeAppService employeeAppService,
            IContractAppService contractAppService, ISitePersonnelLookupAppService sitePersonnelLookupAppService)
        {
            _accountAppService = accountAppService;
            _userAppService = userAppService;
            _employeeAppService = employeeAppService;
            _contractAppService = contractAppService;
            _sitePersonnelLookupAppService = sitePersonnelLookupAppService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            // clear auth cookie
            AuthHelper.SignOut();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(UserLoginModel userLoginModel)
        {
            try
            {
                // does user exist?
                var userDto = _userAppService.VerifyUser(userLoginModel.Login);

                // if the user does not exist, return
                if (userDto == null)
                {
                    return BadRequest("NO USER");
                }

                // if user does exist in DB, verify entered password
                var passwordVerified = _userAppService.VerifyPassword(userLoginModel.Password, userDto.Password);

                if (!passwordVerified)
                {
                    return BadRequest("WRONG PASSWORD");
                }

                var isUserAccountLocked = _userAppService.IsAccountLocked(userLoginModel.Login);
                if (isUserAccountLocked)
                {
                    return BadRequest("ACCOUNT LOCKED");
                }

                // set the auth cookie
                AuthHelper.SetAuthCookie(userDto.Id);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Email/Check")]
        public IHttpActionResult CheckEmailExists(CheckEmailModel checkEmailModel)
        {
            try
            {
                if (_accountAppService.CheckIfEmailExists(checkEmailModel.Email))
                {
                    return BadRequest("Email already taken");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("PasswordReset")]
        public IHttpActionResult ResetPassword(PasswordResetModel passwordResetModel)
        {
            try
            {
                var passwordReset = _userAppService.PasswordReset(passwordResetModel.Email);

                if (!passwordReset)
                {
                    return BadRequest("ITs A TRAP!");
                }

                return Ok("Password reset and email sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("PasswordUpdate")]
        public IHttpActionResult UpdatePassword(UserModel userModel)
        {
            try
            {
                var userDto = Mapper.Map<UserDto>(userModel);
                _accountAppService.UpdateUserPassword(userDto, userDto.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // CRUD
        [Authorize]
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(AdminAreaUserViewModel viewModel)
        {
            try
            {
                //need to chop up the different parts of the view model to map to the correct
                // bits of the relevant DTOs
                long currentUserId = AuthHelper.GetCurrentUserId();
                long userId = 0;
                long? employeeId = null;

                // USER DTO
                if (viewModel.UserModel != null)
                {
                    var userDto = Mapper.Map<UserDto>(viewModel.UserModel);

                    // check if the login and email have already been used
                    if (_accountAppService.CheckIfLoginExists(userDto.Login))
                    {
                        return BadRequest("Login already taken");
                    }

                    if (_accountAppService.CheckIfEmailExists(userDto.Email))
                    {
                        return BadRequest("Email already taken");
                    }

                    if (userDto.ExternalTimeSystemId != null)
                    {
                        if (_accountAppService.CheckIfExternalTimeSystemIdExists((int)userDto.ExternalTimeSystemId))
                        {
                            return BadRequest("ZK Time System ID already taken");
                        }    
                    }

                    userId = _accountAppService.RegisterUserAccount(userDto, currentUserId);
                }


                // EMPLOYEE DTO
                if (viewModel.EmployeeModel != null)
                {
                    var employeeDto = Mapper.Map<EmployeeDto>(viewModel.EmployeeModel);
                    employeeDto.UserId = userId;
                    //TEMP
                    employeeDto.StartDate = new DateTime(1900, 1, 1, 0, 0, 0);
                    employeeDto.LeaveDate = new DateTime(1900, 1, 1, 0, 0, 0);

                    // save and return new id
                    employeeId = _employeeAppService.Create(employeeDto, currentUserId);
                }


                // CONTRACT DTOs
                if (viewModel.ContractModel != null)
                {
                    var contractDto = Mapper.Map<ContractDto>(viewModel.ContractModel);
                    contractDto.EmployeeId = employeeId;
                    //TEMP
                    contractDto.StartDate = new DateTime(1900, 1, 1, 0, 0, 0);
                    contractDto.EndDate = new DateTime(1900, 1, 1, 0, 0, 0);

                    _contractAppService.Create(contractDto, currentUserId);
                }


                // SITE ACCESS
                if (viewModel.SiteAccessModels.Any())
                {
                    var sitePersonnelLookupDtos = new List<SitePersonnelLookupDto>();
                    foreach (var siteAccessModel in viewModel.SiteAccessModels)
                    {
                        siteAccessModel.EmployeeId = employeeId;

                        var sitePersonnelLookupDto = Mapper.Map<SitePersonnelLookupDto>(siteAccessModel);
                        sitePersonnelLookupDtos.Add(sitePersonnelLookupDto);
                    }

                    // send off to be saved
                    foreach (var sitePersonnelLookupDto in sitePersonnelLookupDtos)
                    {
                        _sitePersonnelLookupAppService.Create(sitePersonnelLookupDto, currentUserId);
                    }
                }

                return Ok("User Registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
