using AutoMapper;
using ItemBookingApp_API.Areas.OrganisationAdmin.Resources.Role;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.Areas.OrganisationAdmin.Controllers
{
    [Route("organisation-admin/api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessOrganizationOwnerArea)]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var rolesToReturn = _mapper.Map<IEnumerable<RoleResource>>(roles);

            return Ok(rolesToReturn);
        }
    }
}
