namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class BaseQuery
    {
        private const int MaxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string? FilterBy { get; set; }

        public string? SearchString { get; set; }
    }
}
