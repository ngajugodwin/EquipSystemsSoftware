namespace ItemBookingApp_API.Domain.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public bool IsActive { get; set; }

        public ItemType()
        {
            CreatedAt = DateTime.Now;
            Items = new HashSet<Item>();
        }
    }
}
