using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IOrganisationRepository
    {
        Task<PagedList<Organisation>> ListAsync(OrganisationQuery organisationQuery);

        Task<IEnumerable<Organisation>> ListAsync();

        Task<bool> IsExist(string organisationName);

        Task<bool> IsActive(int organisationId);
    }
}
