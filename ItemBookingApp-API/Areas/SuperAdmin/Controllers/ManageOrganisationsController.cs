using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Areas.Resources.Organisation;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Areas.SuperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageOrganisationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrganisationService _organisationService;
        private readonly IGenericRepository _genericRepository;

        public ManageOrganisationsController(IMapper mapper, IOrganisationService organisationService, IGenericRepository genericRepository)
        {
            _mapper = mapper;
            _organisationService = organisationService;
            _genericRepository = genericRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] OrganisationQueryResource organisationQueryResource)
        {
            var organisationQuery = _mapper.Map<OrganisationQueryResource, OrganisationQuery>(organisationQueryResource);

            var organisations = await _organisationService.ListAsync(organisationQuery);

            var organisationsToReturn = _mapper.Map<IEnumerable<OrganisationResource>>(organisations);

            Response.AddPagination(organisations.CurrentPage, organisations.PageSize, organisations.TotalCount, organisations.TotalPages);

            return Ok(organisationsToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganisationAsync([FromBody] SaveOrganisationResource saveOrganisationResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var organisationToSave = _mapper.Map<SaveOrganisationResource, Organisation>(saveOrganisationResource);

            var result = await _organisationService.SaveAsync(organisationToSave);

            if (!result.Success)
                return BadRequest(result.Message);

            var organisationToReturn = _mapper.Map<OrganisationResource>(result.Resource);

            return Ok(organisationToReturn);
        }

        [HttpPut("{organisationId}")]
        public async Task<IActionResult> UpdateOrganisationAsync(int organisationId, [FromBody] UpdateOrganisationResource updateOrganisationResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (organisationId != updateOrganisationResource.Id)
                return BadRequest("Id does not match request body");

            var organisation = _mapper.Map<UpdateOrganisationResource, Organisation>(updateOrganisationResource);

            var result = await _organisationService.UpdateAsync(organisationId, organisation);

            if (!result.Success)
                return BadRequest(result.Message);

            var updatedOrganisationToReturn = _mapper.Map<OrganisationResource>(result.Resource);

            return Ok(updatedOrganisationToReturn);
        }

        [HttpGet("{organisationId}", Name = "GetOrgaisationAsync")]
        public async Task<IActionResult> GetOrgaisationAsync(int organisationId)
        {
            var result = await _genericRepository.FindAsync<Organisation>(x => x.Id == organisationId);

            var organisationToReturn = _mapper.Map<OrganisationResource>(result);

            return Ok(organisationToReturn);
        }

        [HttpPut("{organisationId}/setOrganisationStatus")]
        public async Task<IActionResult> SetOrganisationStatus(int organisationId, bool organisationStatus)
        {
            var result = await _organisationService.ActivateOrDeactivateOrganisation(organisationId, organisationStatus);

            if (!result.Success)
                return BadRequest(result.Message);

            var updatedOrganisation = _mapper.Map<OrganisationResource>(result.Resource);

            return Ok(updatedOrganisation);
        }
    }
}
