using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountsController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateNewAccountAsync([FromBody] SaveUserResource saveUserResource, [FromQuery] bool isExternalReg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var userRoles = new List<string>();

            if (saveUserResource.Roles != null && saveUserResource.Roles.Length > 0)
            {
                foreach (var roleName in saveUserResource.Roles)
                {
                    userRoles.Add(roleName);
                }
            }

            var userToSave = _mapper.Map<SaveUserResource, AppUser>(saveUserResource);

            var result = await _userService.SaveAsync(userToSave, isExternalReg, userRoles, saveUserResource.Password);

            if (!result.Success)
            {
                var errorData = new
                {
                    Message = result.Message,
                    Error = result.Errors
                };
                return BadRequest(errorData);
            }

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
            //  return CreatedAtRoute("GetUserAsync", new { controller = "Users", id = userToReturn.Id }, userToReturn);
        }
    }
}
