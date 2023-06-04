namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class UserQuery : BaseQuery
    {
        public int OrganisationId { get; set; }

        public long UserId { get; set; }
    }
}
