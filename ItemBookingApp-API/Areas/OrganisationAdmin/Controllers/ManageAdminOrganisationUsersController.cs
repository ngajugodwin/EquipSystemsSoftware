using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.SelfService;
using ItemBookingApp_API.Services;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Areas.OrganisationAdmin.Controllers
{
    [Route("organisation-admin/api/[controller]/{organisationId}")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessOrganizationOwnerArea)]
    public class ManageAdminOrganisationUsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IManageAdminOrganisationService _manageAdminOrganisationService;

        public ManageAdminOrganisationUsersController(IMapper mapper, IManageAdminOrganisationService manageAdminOrganisationService)
        {
            _mapper = mapper;
            _manageAdminOrganisationService = manageAdminOrganisationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(int organisationId, [FromBody] SaveUserResource saveUserResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var userRoles = new List<string>();

            if (saveUserResource.Roles?.Length > 0)
            {
                foreach (var roleName in saveUserResource.Roles)
                {
                    userRoles.Add(roleName);
                }
            }

            var userToSave = _mapper.Map<SaveUserResource, AppUser>(saveUserResource);

            var result = await _manageAdminOrganisationService.SaveAsync(organisationId, userToSave, userRoles, saveUserResource.Password);

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
            //var rout = $"ManageAdminOrganisationUsers/{userToReturn.OrganisationId}";

            //return CreatedAtRoute("GetOrgUserAsync", new { controller = "ManageAdminOrganisationUsers", id = userToReturn.Id }, userToReturn);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(int organisationId, long userId, [FromBody] UpdateUserResource updateUserResource)
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

            var result = await _manageAdminOrganisationService.UpdateUserAsync(organisationId, userId, user, userRoles);

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

        [HttpGet]
        public async Task<IActionResult> ListAsync(int organisationId, [FromQuery] UserQueryResource userQueryResource)
        {
            var userQuery = _mapper.Map<UserQueryResource, UserQuery>(userQueryResource);

            var users = await _manageAdminOrganisationService.GetOrganisationUsersListAsync(organisationId, userQuery);

            var usersToReturn = _mapper.Map<IEnumerable<UserResource>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        //[HttpGet("{id}", Name = "GetOrganisationUserAsync")]
        //public async Task<IActionResult> GetOrganisationUserAsync(int organisationId, long id)
        //{
        //    var result = await _manageAdminOrganisationService.GetUserByIdAsync(id, organisationId);

        //    if (!result.Success)
        //        return BadRequest(result.Message);

        //    var userToReturn = _mapper.Map<UserResource>(result.Resource);

        //    return Ok(userToReturn);
        //}


        [HttpGet("{id}", Name = "GetOrgUserAsync")]
        public async Task<IActionResult> GetOrgUserAsync(int organisationId, long id)
        {
            var result = await _manageAdminOrganisationService.GetUserByIdAsync(id, organisationId);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }


        [HttpPut("{userId}/setUserStatus")]
        public async Task<IActionResult> ActivateOrDisableUser(int organisationId, long userId, [FromQuery] bool userStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _manageAdminOrganisationService.ActivateOrDisableUser(organisationId, userId, userStatus);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }

        [HttpPut("{userId}/changeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword(int organisationId, long userId, [FromBody] PasswordUpdateResource passwordUpdateResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _manageAdminOrganisationService.ChangePassword(organisationId, userId, passwordUpdateResource.OldPassword, passwordUpdateResource.NewPassword, true);

            if (!result.Success)
                return BadRequest(result.Message);

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(userToReturn);
        }
    }
}
