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
}
