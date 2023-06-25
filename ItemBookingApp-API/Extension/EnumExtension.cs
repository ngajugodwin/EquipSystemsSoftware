using ItemBookingApp_API.Domain.Models.OrderAggregate;
using System.ComponentModel;

namespace ItemBookingApp_API.Extension
{
    public static class EnumExtension
    {
        public static string ToDescriptionString<TEnum>(this TEnum enumType)
        {
            var info = enumType.GetType().GetField(enumType.ToString());

            var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?[0].Description ?? enumType.ToString();
        }

        public static string ToModelDescriptionString<ApprovalStatus>(this BookingInformation bookingInformation)
        {
            if (bookingInformation == null)
                return string.Empty;

            var info = bookingInformation.Status.GetType().GetField(bookingInformation.Status.ToString());

            var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            var result =  attributes?[0].Description ?? bookingInformation.Status.ToString();

            return result;
        }
    }
}
