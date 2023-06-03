using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Resources.Auth
{
    public class LoginResource
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
