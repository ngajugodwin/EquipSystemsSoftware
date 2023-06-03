using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Areas.Resources.Category
{
    public class CategoryQueryResource : BaseQuery
    {
        public bool CategoryStatus { get; set; }
    }
}
