using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class SaveItemResource
    {
        [Required]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        [Required]
        public string SerialNumber { get; set; }


        public SaveItemResource()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
