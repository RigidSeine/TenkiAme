namespace TenkiAme.UtilityObjects
{
    public static class DateUtil
    {
        public static DateTime TruncateToYearStart(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }
        public static DateTime TruncateToMonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        public static DateTime TruncateToDayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
        public static DateTime TruncateToHourStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }
        public static DateTime TruncateToMinuteStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }
        public static DateTime TruncateToSecondStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        public static DateTime GetCurrentNZDateTime()
        {
            //Get current timezone of machine and calculate the current time in NZST
            var currentTimeZone = TimeZoneInfo.Local;
            var NZST = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            var currentNZTime = TimeZoneInfo.ConvertTime(DateTime.Now, currentTimeZone, NZST);

            return currentNZTime;
        }
    }
}
