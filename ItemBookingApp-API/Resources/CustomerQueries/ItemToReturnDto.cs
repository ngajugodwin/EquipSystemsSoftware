using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Resources.CustomerQueries
{
    public class ItemToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string SerialNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ItemState ItemState { get; set; }

        public string ItemType { get; set; }

        public string Category { get; set; }

        public string? Url { get; set; }
        public string? PublicId { get; set; }
    }
}
