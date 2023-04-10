using TimeZoneConverter;

namespace GlobalUtilities
{
    public static class DateConverter
    {
        public static DateTime ToLocal(DateTime utcDateTime, string locaTimezone)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified);
            // TimeZoneInfo timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById(locaTimezone);
            TimeZoneInfo timezoneInfo = TZConvert.GetTimeZoneInfo(locaTimezone);
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timezoneInfo);

            return localDateTime;
        }

        public static DateTime ToUTC(DateTime localDateTime, string locaTimezone)
        {
            localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
          //  TimeZoneInfo timezoneInfo =  TimeZoneInfo.FindSystemTimeZoneById(locaTimezone);
            TimeZoneInfo timezoneInfo = TZConvert.GetTimeZoneInfo(locaTimezone);

            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, timezoneInfo);

            //DateTime.SpecifyKind(test, DateTimeKind.Utc);
            return utcTime;
        }
    }
}
