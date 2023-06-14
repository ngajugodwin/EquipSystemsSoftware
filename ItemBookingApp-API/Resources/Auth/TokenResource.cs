using ItemBookingApp_API.Areas.Resources.AppUser;

namespace ItemBookingApp_API.Resources.Auth
{
    public class TokenResource
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; }

        public UserResource User { get; set; }

        public int OrganisationId { get; set; }
    }
}
