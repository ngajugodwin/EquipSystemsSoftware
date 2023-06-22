using System.Security.Claims;

namespace ItemBookingApp_API.Extension
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            var email =  user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return email ?? string.Empty;
        }
    }
}
