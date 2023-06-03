using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.AppUser
{
    public class SaveUserResource
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string[] Roles { get; set; }
    }
}
