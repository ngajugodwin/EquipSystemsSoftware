using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.ItemType
{
    public class ItemTypeQueryResource : BaseQuery
    {
        public string Status { get; set; } = string.Empty;
    }
}
