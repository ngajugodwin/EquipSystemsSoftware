namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class OrderReportQuery : BaseQuery
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
