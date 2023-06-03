using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.Category
{
    public class SaveCategoryResource
    {
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public SaveCategoryResource()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
