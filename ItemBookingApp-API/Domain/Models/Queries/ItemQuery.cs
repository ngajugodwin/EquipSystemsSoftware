namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class ItemQuery : BaseQuery
    {
        public string Status { get; set; } = string.Empty;
        public int ItemState { get; set; }
    }
}
