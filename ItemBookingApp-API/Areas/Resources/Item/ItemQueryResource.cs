using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class ItemQueryResource : BaseQuery
    {
        public string Status { get; set; } = string.Empty;
        public int ItemState { get; set; }
    }
}
