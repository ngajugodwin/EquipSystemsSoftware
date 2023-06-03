using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class ItemQueryResource : BaseQuery
    {
        public bool IsActive { get; set; } = false;
        public int ItemState { get; set; }
    }
}
