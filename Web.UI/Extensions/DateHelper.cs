using System.Globalization;

namespace Web.UI.Extensions
{
    public static class DateHelper
    {
        public static string Format(this DateTime dateTime, DateTime value, string format, bool isDisplayTime)
        {
            if (isDisplayTime)
            {
                return value.ToString(format, CultureInfo.InvariantCulture);
            }
            else
            {
                return value.ToString(format, CultureInfo.InvariantCulture);
            }
        }
    }
}
