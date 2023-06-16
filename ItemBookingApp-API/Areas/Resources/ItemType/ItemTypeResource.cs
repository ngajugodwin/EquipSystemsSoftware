namespace ItemBookingApp_API.Areas.Resources.ItemType
{
    public class ItemTypeResource
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = string.Empty;

        public int CategoryId { get; set; }
    }
}
