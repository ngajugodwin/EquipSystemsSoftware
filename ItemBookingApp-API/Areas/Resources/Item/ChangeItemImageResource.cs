namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class ChangeItemImageResource
    {
        public string? Url { get; set; }

        public IFormFile File { get; set; }

        public string? PublicId { get; set; }

        public int ItemId { get; set; }
    }
}
