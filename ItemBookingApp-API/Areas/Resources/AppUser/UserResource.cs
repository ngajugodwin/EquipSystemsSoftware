using ItemBookingApp_API.Areas.Resources.Organisation;

namespace ItemBookingApp_API.Areas.Resources.AppUser
{
    public class UserResource
    {
        public long Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string[] UserRoles { get; set; }

        public string Status { get; set; } = string.Empty;

        public string TypeName { get; set; } = string.Empty;

        public string? OrganisationId { get; set; }

        public OrganisationResource OrganisationResource { get; set; }
    }
}
