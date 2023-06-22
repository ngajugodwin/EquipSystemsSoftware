namespace ItemBookingApp_API.Domain.Models.OrderAggregate
{
    public class Order
    {
        public Order()
        {

        }
        public Order(IReadOnlyList<OrderItem> orderItems, string borrowerEmail, Address shipToAddress, 
            DeliveryMethod deliveryMethod, 
            decimal subTotal)
        {
            //Id = id;
            BorrowerEmail = borrowerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
        }

        public int Id { get; set; }
        public string BorrowerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        public decimal SubTotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string PaymentItentId { get; set; }

        public decimal GetTotal () 
        {
            return SubTotal + DeliveryMethod.Price;
        }
    }
}
