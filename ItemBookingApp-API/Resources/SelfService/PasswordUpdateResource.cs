using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Resources.SelfService
{
    public class PasswordUpdateResource
    {
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
