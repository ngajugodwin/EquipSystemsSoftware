using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Resources.CustomerQueries
{
    public class CustomerItemQuery : BaseQuery
    {
        public int CategoryId { get; set; }

        public int ItemTypeId { get; set; }

        public string Sort { get; set; } = "priceAsc";
    }
}
