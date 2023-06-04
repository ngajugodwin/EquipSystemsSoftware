using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using ItemBookingApp_API.Resources.Query;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class OrganisationRepository : BaseRepository, IOrganisationRepository
    {
        public OrganisationRepository(ApplicationDbContext context):
            base(context)
        {

        }
        public async Task<bool> IsActive(int organisationId)
        {
            var organisation = await _context.Organisations.FindAsync(organisationId);

            return (organisation != null && organisation.IsActive) ? true : false;
        }

        public async Task<bool> IsExist(string organisationName)
        {
            var organisation = await _context.Organisations
                    .AnyAsync(c => c.Name.ToLower() == organisationName.ToLower());

            return (organisation) ? true : false;
        }

        public Task<PagedList<Organisation>> ListAsync(OrganisationQuery organisationQuery)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Organisation>> ListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
