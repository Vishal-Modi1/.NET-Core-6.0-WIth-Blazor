using System.Globalization;

namespace GlobalUtilities.Extensions
{
    public static class DateHelper
    {
        public static string SetCustomFormat(this DateTime dateTime, DateTime value, string format, bool isDisplayTime)
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
