using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace ItemBookingApp_API.Domain.Models.Identity
{
    public class AppUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public override string Email { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }

        public AppUser()
        {
            CreatedAt = DateTime.Now;
            Tokens = new HashSet<Token>();
        }
    }
}
