using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.Category
{
    public class CategoryQueryResource : BaseQuery
    {
        public EntityStatus Status { get; set; }
    }
}
