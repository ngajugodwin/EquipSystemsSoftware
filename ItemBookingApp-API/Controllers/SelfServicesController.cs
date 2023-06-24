using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.SelfService;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ItemBookingApp_API.Controllers
{
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.HasUserRole)]
    public class SelfServicesController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public SelfServicesController(IMapper mapper,
        IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getUserAddress")]
        public async Task<IActionResult> GetUserAddress(long userId)
        {
            var userAddressToReturn = await _userService.GetUserAddress(userId);

            return Ok(userAddressToReturn);
        }

        [HttpPut("updateUserAddress")]
        public async Task<IActionResult> UpdateUserAddress(long userId, UserAddress userAddress)
        {
            var updatedUserAddress = await _userService.UpdateUserAddress(userId, userAddress);

            return Ok(updatedUserAddress);
        }


        [HttpGet("getUserProfile")]
        public async Task<IActionResult> GetUserProfile(long userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> ChangePasswordAsync(long userId, PasswordUpdateResource passwordUpdateResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userService.ChangePassword(userId, passwordUpdateResource.OldPassword, passwordUpdateResource.NewPassword, false);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }

    }
}
