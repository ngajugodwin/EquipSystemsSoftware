using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.AppUser
{
    public class UserQueryResource : BaseQuery
    {
        public int OrganisationId { get; set; }

        //  public long UserId { get; set; }

        public AccountType AccountType { get; set; }

        public string? AccountTypeName { get; set; }

        public EntityStatus Status { get; set; }
    }
}
