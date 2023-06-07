using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.Organisation
{
    public class OrganisationQueryResource : BaseQuery
    {
        public EntityStatus Status { get; set; }
    }
}
