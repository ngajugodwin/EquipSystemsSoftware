namespace ItemBookingApp_API.Domain.Models.OrderAggregate
{
    public class OrderItem
    {
        public OrderItem()
        {

        }

        public OrderItem(ItemOrdered itemOrdered, decimal price, int quantity)
        {
            //Id = id;
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public ItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
