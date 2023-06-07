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

            return (organisation != null && organisation.Status == EntityStatus.Active) ? true : false;
        }

        public async Task<bool> IsExist(string organisationName)
        {
            var organisation = await _context.Organisations
                    .AnyAsync(c => c.Name.ToLower() == organisationName.ToLower());

            return (organisation) ? true : false;
        }

        public async Task<PagedList<Organisation>> ListAsync(OrganisationQuery organisationQuery)
        {
            var organisations = Enumerable.Empty<Organisation>().AsQueryable();

            if (organisationQuery.Status > 0)
            {
                switch (organisationQuery.Status)
                {
                    case EntityStatus.Pending:
                        organisations = _context.Organisations.Where(o => o.Status == EntityStatus.Pending);
                        break;
                    case EntityStatus.Active:
                        organisations = _context.Organisations.Where(o => o.Status == EntityStatus.Active);
                        break;
                    case EntityStatus.Disabled:
                        organisations = _context.Organisations.Where(o => o.Status == EntityStatus.Disabled);
                        break;
                    default:
                        organisations = _context.Organisations.AsQueryable();
                        break;
                }
            }
            else
            {
                organisations = _context.Organisations.AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(organisationQuery.SearchString))
            {
                organisations = organisations.Where(c => c.Name.Contains(organisationQuery.SearchString));
            }

            organisations = organisations.OrderByDescending(c => c.CreatedAt);

            return await PagedList<Organisation>.CreateAsync(organisations, organisationQuery.PageNumber, organisationQuery.PageSize);
        }

        public async Task<IEnumerable<Organisation>> ListAsync()
        {
            return await _context.Organisations.ToListAsync();
        }
    }
}
