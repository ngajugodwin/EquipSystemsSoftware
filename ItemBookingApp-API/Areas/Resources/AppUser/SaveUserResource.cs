using ItemBookingApp_API.Areas.Resources.Organisation;
using ItemBookingApp_API.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.AppUser
{
    public class SaveUserResource
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string[]? Roles { get; set; }

        public Domain.Models.Organisation? Organisation { get; set; }

        public int AccountType { get; set; }

        // public SaveOrganisationResource SaveOrganisationResource { get; set; }


    }
}
