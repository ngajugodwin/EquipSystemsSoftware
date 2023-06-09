﻿using System.ComponentModel;

namespace ItemBookingApp_API.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string SerialNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool? IsActive { get; set; }

        public ItemState ItemState { get; set; }

        public int ItemTypeId { get; set; }
        public virtual ItemType ItemType { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string? Url { get; set; }
        public string? PublicId { get; set; }

        public decimal Price { get; set; }

        public int AvailableQuantity { get; set; }

        public string Description { get; set; }

        public virtual ICollection<BasketItem> BasketItems { get; set; }

        public Item()
        {
            BasketItems = new HashSet<BasketItem>();
        }
    }

    public enum ItemState 
    {
        [Description("Not Available")]
        NotAvailable = 0,

        [Description("Available")]
        Available = 1,

        [Description("Borrowed")]
        Borrowed = 2, 
    }

}
