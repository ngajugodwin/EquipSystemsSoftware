namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class ItemResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int AvailableQuantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }

        public int CategoryId { get; set; }

        public string ItemState { get; set; }

        public string SerialNumber { get; set; }

        public string? Url { get; set; }

        public string? PublicId { get; set; }
    }
}
