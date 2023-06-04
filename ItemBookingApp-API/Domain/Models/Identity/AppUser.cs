using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace ItemBookingApp_API.Domain.Models.Identity
{
    public class AppUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public override string Email { get; set; }

        public bool IsPrimaryOrganisationContact { get; set; }

        public AccountType AccountType { get; set; }

        public virtual Organisation Organisation { get; set; }

        public int? OrganisationId { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }

        public AppUser()
        {
            CreatedAt = DateTime.Now;
            Tokens = new HashSet<Token>();
        }       
    }

    public enum AccountType
    {
        [Description("Individual")]
        Individual = 1,

        [Description("Organisation")]
        Organisation = 2,

        [Description("Master")]
        Master = 3,
    }
}
