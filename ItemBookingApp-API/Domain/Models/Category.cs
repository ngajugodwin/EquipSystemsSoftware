namespace ItemBookingApp_API.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public EntityStatus Status { get; set; }
        public virtual ICollection<ItemType> ItemTypes { get; set; }

        public Category()
        {
            ItemTypes = new HashSet<ItemType>();
        }

    }
}
