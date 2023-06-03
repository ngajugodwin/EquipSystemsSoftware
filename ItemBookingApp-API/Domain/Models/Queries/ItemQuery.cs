namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class ItemQuery : BaseQuery
    {
        public bool IsActive { get; set; } = false;
        public int ItemState { get; set; }
    }
}
