using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
using RMS.WebAPI.Helpers;
using RMS.WebAPI.Models;
using RMS.WebAPI.ViewModels;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/User")]
    public class UsersController : ApiController
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        // Repo Methods
        [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var userDto = _userAppService.GetById(id);
                var userModel = Mapper.Map<UserModel>(userDto);

                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var userDtos = _userAppService.GetAll();
                var userModels = new List<UserModel>();

                foreach (var userDto in userDtos)
                {
                    userModels.Add(Mapper.Map<UserModel>(userDto));
                }

                return Ok(userModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllActive")]
        public IHttpActionResult GetAllActive()
        {
            try
            {
                var userDtos = _userAppService.GetAllActive();
                var userModels = new List<UserModel>();

                foreach (var userDto in userDtos)
                {
                    userModels.Add(Mapper.Map<UserModel>(userDto));
                }

                return Ok(userModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllInactive")]
        public IHttpActionResult GetAllInactive()
        {
            try
            {
                var userDtos = _userAppService.GetAllInactive();
                var userModels = new List<UserModel>();

                foreach (var userDto in userDtos)
                {
                    userModels.Add(Mapper.Map<UserModel>(userDto));
                }

                return Ok(userModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Profile")]
        public IHttpActionResult GetProfile()
        {
            try
            {
                var userDto = _userAppService.GetById(AuthHelper.GetCurrentUserId());
                var userModel = Mapper.Map<UserModel>(userDto);

                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsernameAndId")]
        public IHttpActionResult GetUsernameAndId()
        {
            try
            {
                var userListItemDtos = _userAppService.GetUsernameAndId();
                var userListItemModels = new List<UserListItemModel>();

                foreach (var userListItem in userListItemDtos)
                {
                    var userListItemModel = new UserListItemModel
                    {
                        Id = userListItem.Id,
                        Firstname = userListItem.Firstname,
                        Lastname = userListItem.Lastname
                    };

                    userListItemModels.Add(userListItemModel);
                }

                return Ok(userListItemModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // CRUD
        [Authorize]
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateUser(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userDto = Mapper.Map<UserDto>(userModel);

                _userAppService.Update(userDto, AuthHelper.GetCurrentUserId());

                return Ok("User Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Delete/{Id}")]
        public IHttpActionResult DeleteUser(long id)
        {
            try
            {
                _userAppService.Delete(id, AuthHelper.GetCurrentUserId());

                return Ok("User Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
