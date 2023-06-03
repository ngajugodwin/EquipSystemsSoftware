namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class UpdateItemResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ItemStateId { get; set; }
        public string SerialNumber { get; set; }
    }
}
