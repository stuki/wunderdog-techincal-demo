using System;

namespace Sula.Core.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static bool IsCloseToMidnight(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.Hour <= 6 || dateTimeOffset.Hour > 18;
        }

        public static DateTimeOffset ToMidnight(this DateTimeOffset dateTimeOffset)
        {
            if (dateTimeOffset.Hour <= 6)
            {
                return new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 0, 0, 0, dateTimeOffset.Offset);
            }

            var nextDay = dateTimeOffset.AddDays(1);

            return new DateTimeOffset(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0, nextDay.Offset);
        }

        public static DateTimeOffset ToMidday(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 12, 0, 0, dateTimeOffset.Offset);
        }

        public static DateTimeOffset ToHour(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, 0, 0, dateTimeOffset.Offset);
        }

        public static DateTimeOffset ToMonth(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, 1, 0, 0, 0, dateTimeOffset.Offset);
        }
    }
}