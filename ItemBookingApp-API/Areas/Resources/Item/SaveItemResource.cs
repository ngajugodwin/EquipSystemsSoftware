using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.Item
{
    public class SaveItemResource
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int AvailableQuantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? Url { get; set; }

        public IFormFile File { get; set; }

        public string? PublicId { get; set; }

        public int CategoryId { get; set; }


        public SaveItemResource()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
