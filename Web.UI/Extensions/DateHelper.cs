using System.Globalization;

namespace Web.UI.Extensions
{
    public static class DateHelper
    {
        public static string Format(this DateTime dateTime, DateTime value, string format, bool isDisplayTime)
        {
            if (isDisplayTime)
            {
                string dateFormat = format.Replace("$", " ");
                return value.ToString(dateFormat, CultureInfo.InvariantCulture);
            }
            else
            {
                string dateFormat = format.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries).First();
             
                return value.ToString(dateFormat, CultureInfo.InvariantCulture);
            }
        }
    }
}
