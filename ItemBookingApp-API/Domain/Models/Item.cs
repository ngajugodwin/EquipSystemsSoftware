﻿using System.ComponentModel;

namespace ItemBookingApp_API.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string SerialNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool? IsActive { get; set; }

        public ItemState ItemState { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
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