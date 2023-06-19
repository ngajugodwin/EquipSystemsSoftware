using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Resources.CustomerQueries
{
    public class CustomerItemTypeQuery : BaseQuery
    {
        public int CategoryId { get; set; }
    }
}
