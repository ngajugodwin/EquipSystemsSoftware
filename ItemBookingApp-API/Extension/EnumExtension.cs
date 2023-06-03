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
    }
}
