namespace ItemBookingApp_API.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Item> Items { get; set; }

        public Category()
        {
            Items = new HashSet<Item>();
        }

    }
}
