using ItemBookingApp_API.Domain.Models.Identity;

namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class UserQuery : BaseQuery
    {
        public int OrganisationId { get; set; }

      //  public long UserId { get; set; }

        public AccountType AccountType { get; set; }

        public string AccountTypeName { get; set; } = string.Empty;

        public EntityStatus Status { get; set; }

    }
}
