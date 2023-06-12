using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IOrganisationService
    {
        Task<OrganisationResponse> SaveAsync(Organisation organisation);

        Task<OrganisationResponse> UpdateAsync(int organisationId, Organisation organisation);

        Task<OrganisationResponse> ActivateOrDeactivateOrganisation(int organisationId, bool organisationStatus);

        Task<PagedList<Organisation>> ListAsync(OrganisationQuery organisationQuery);


        Task<IEnumerable<Organisation>> ListAsync();

        Task<OrganisationResponse> RejectOrganisation(int organisationId);

    }
}
