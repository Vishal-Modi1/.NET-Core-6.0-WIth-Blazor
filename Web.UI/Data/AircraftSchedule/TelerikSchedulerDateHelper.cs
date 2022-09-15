using Telerik.Blazor;

namespace Web.UI.Data.AircraftSchedule
{
    public static class TelerikSchedulerDateHelper
    {
        public static Tuple<DateTime,DateTime> GetDates(DateTime startDateFromUI, SchedulerView currentView, int multiDayDaysCount)
        {
            // translate UI to date range
            DateTime startDate = startDateFromUI;
            DateTime endDate = startDateFromUI;
            switch (currentView)
            {
                case SchedulerView.Day:
                    break;
                case SchedulerView.MultiDay:
                    endDate = endDate.AddDays(multiDayDaysCount);
                    break;
                case SchedulerView.Week:
                    startDate = GetCurrentWeekStartTime(startDate);
                    endDate = startDate.AddDays(7);
                    break;
                case SchedulerView.Month:
                    // if adjacent months are visible, it is up to +/- 6 days
                    // to optimize futher you'd have to write a lot of calendar logic to find
                    // what day of the week the 1st of the month is, and how many days from the previous
                    // and next month are visible, which can make for convoluted and complex code
                    startDate = startDateFromUI;
                    // make it even simpler - no months are shorter than 28 days, none is longer than 31
                    // so adding 9 adds at least 6 to the longest possible for the case where most days are seen
                    endDate = startDateFromUI.AddMonths(1);
                    break;
                case SchedulerView.Timeline:
                    endDate = endDate.AddDays(1);
                    break;
                default:
                    throw new ArgumentException("the service does not know how to handle this scheduler view yet");
            }

            return Tuple.Create(startDate, endDate);
        }

        private static DateTime GetCurrentWeekStartTime()
        {
            return GetCurrentWeekStartTime(DateTime.Now);
        }

        private static DateTime GetCurrentWeekStartTime(DateTime currTime)
        {
            DateTime now = currTime;
            int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime lastMonday = now.AddDays(-1 * diff);
            // return 8 AM on today's date for better visualization of the demos
            return new DateTime(lastMonday.Year, lastMonday.Month, lastMonday.Day, 8, 0, 0);
        }
    }
}
