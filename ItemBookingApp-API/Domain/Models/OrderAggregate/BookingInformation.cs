namespace ItemBookingApp_API.Domain.Models.OrderAggregate
{
    public class BookingInformation
    {
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public DateTimeOffset? ReturnedDate { get; set; }

        public BookingInformation()
        {

        }

        public BookingInformation(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
