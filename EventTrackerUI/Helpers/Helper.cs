using System;
namespace EventTrackerUI.Helpers
{
    public static class Helper
    {
        public static string GetTimeToDate(DateOnly _date)
        {
            int days = GetDaysBetweenDateAndToday(_date);

            if (days == 0)
            {
                return "Today";
            }

            return DaysDiffToString(days) + " ago";
        }

        private static int GetDaysBetweenDateAndToday(DateOnly _date)
        {
            return DateOnly.FromDateTime(DateTime.Now.Date).DayNumber - _date.DayNumber;
        }

        public static int GetDaysBetweenDates(DateOnly current, DateOnly past)
        {
            return current.DayNumber - past.DayNumber;
        }

        public static string DaysDiffToString(int days)
        {
            if (days > 365)
            {
                return $"{days / 365}y";
            }
            else if (days > 30)
            {
                return $"{days / 30}m";
            }
            else
            {
                return $"{days}d";
            }
        }
    }
}
