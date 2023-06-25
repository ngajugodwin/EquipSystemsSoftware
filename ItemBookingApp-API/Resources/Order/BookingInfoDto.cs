namespace ItemBookingApp_API.Resources.Order
{
    public class BookingInfoDto
    {
        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public DateTimeOffset? ReturnedDate { get; set; }

        public string? BookingStatus { get; set; }
    }
}
