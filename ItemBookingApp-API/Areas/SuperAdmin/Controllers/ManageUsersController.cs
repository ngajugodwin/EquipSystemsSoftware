using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.SelfService;
using ItemBookingApp_API.Services;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Areas.SuperAdmin.Controllers
{
    [Route("super-admin/api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessSuperAdminArea)]
    public class ManageUsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ManageUsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }


        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(long userId, [FromBody] UpdateUserResource updateUserResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (userId != updateUserResource.Id)
                return BadRequest();

            var userRoles = new List<string>();

            if (updateUserResource.Roles.Length > 0)
            {
                foreach (var roleName in updateUserResource.Roles)
                {
                    userRoles.Add(roleName);
                }
            }

            var user = _mapper.Map<UpdateUserResource, AppUser>(updateUserResource);

            var result = await _userService.UpdateAsync(userId, user, userRoles);

            if (!result.Success)
            {
                var errorData = new
                {
                    Message = result.Message,
                    Error = result.Errors.Select(x => x.Description)
                };
                return BadRequest(errorData);
            }

            var updatedUserToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(updatedUserToReturn);
        }

        [HttpGet("{id}", Name = "GetUserAsync")]
        public async Task<IActionResult> GetUserAsync(long id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }

        [HttpPut("{userId}/activateOrDisableUser")]
        public async Task<IActionResult> ActivateOrDisableUser(long userId, [FromQuery]bool userDeactivationStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userService.ActivateOrDisableUser(userId, userDeactivationStatus);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }


        [HttpPut("{userId}/changeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword(long userId, [FromBody] PasswordUpdateResource passwordUpdateResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userService.ChangePassword(userId, passwordUpdateResource.OldPassword, passwordUpdateResource.NewPassword, true);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }

    }
}
