namespace ItemBookingApp_API.Areas.Resources.AppUser
{
    public class UserResource
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string[] UserRoles { get; set; }

        public string Status { get; set; }
    }
}
