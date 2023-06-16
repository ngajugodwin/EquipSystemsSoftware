namespace ItemBookingApp_API.Areas.Resources.ItemType
{
    public class UpdateItemTypeResource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
