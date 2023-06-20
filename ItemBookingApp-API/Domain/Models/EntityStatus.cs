using System.ComponentModel;

namespace ItemBookingApp_API.Domain.Models
{
    public enum EntityStatus
    {

        [Description("Pending")]
        Pending = 1,

        [Description("Active")]
        Active = 2,

        [Description("Disabled")]
        Disabled = 3,
    }

    public enum Status : byte
    {
        [Description("Pending")]
        Pending = 0,

        [Description("Approved")]
        Approved = 1,

        [Description("Closed")]
        Closed = 2

    }
}
