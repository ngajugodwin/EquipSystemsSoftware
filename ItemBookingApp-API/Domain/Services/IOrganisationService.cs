using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Services.Communication;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IOrganisationService
    {
        Task<OrganisationResponse> SaveAsync(Organisation organisation);

        Task<OrganisationResponse> UpdateAsync(int organisationId, Organisation organisation);

        Task<OrganisationResponse> ActivateOrDeactivateOrganisation(int organisationId, bool organisationStatus);

    }
}
