namespace ItemBookingApp_API.Domain.Models.OrderAggregate
{
    public class ItemOrdered
    {
        public ItemOrdered()
        {

        }

        public ItemOrdered(int itemId, string name, string pictureUrl)
        {
            ItemId = itemId;
            Name = name;
            PictureUrl = pictureUrl;
        }

        public int ItemId { get; set; }
        public string Name { get; set; }

        public string PictureUrl { get; set; }
    }
}
