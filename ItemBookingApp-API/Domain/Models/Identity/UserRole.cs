using Microsoft.AspNetCore.Identity;

namespace ItemBookingApp_API.Domain.Models.Identity
{
    public class UserRole: IdentityUserRole<long>
    {
        public AppUser AppUser { get; set; }
        public Role Role { get; set; }
    }
}
