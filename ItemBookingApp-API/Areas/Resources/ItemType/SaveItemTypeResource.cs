using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.ItemType
{
    public class SaveItemTypeResource
    {

        [Required]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public SaveItemTypeResource()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
