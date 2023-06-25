using System.ComponentModel;
using System.Runtime.Serialization;

namespace ItemBookingApp_API.Domain.Models.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending,

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,

        [EnumMember(Value ="Payment Failed")]
        PaymentFailed,
      
    }

    public enum ApprovalStatus
    {
        [Description("Pending")]
        Pending,

        [Description("Approved")]
        Approved,

        [Description("Closed")]
        Closed
    }
}
