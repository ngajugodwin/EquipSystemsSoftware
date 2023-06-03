using ItemBookingApp_API.Domain.Models.Identity;

namespace ItemBookingApp_API.Domain.Models
{
    public class Token
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public string Value { get; set; }

        public long UserId { get; set; }

        public virtual AppUser User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiryTime { get; set; }
    }
}
