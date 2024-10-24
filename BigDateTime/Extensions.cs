using System.Numerics;
using ExtendedNumerics;

namespace BigTime;

using static GregorianCalendarConstants;

internal static class Extensions {
    /// <summary>
    /// Returns the total number of seconds since 0000/00/00 00:00:00.
    /// </summary>
    public static BigDecimal TotalSeconds(this DateTime DateTime) {
        return DateTime.Subtract(DateTime.MinValue).TotalSeconds
            + SecondsInCommonYear; // Add seconds in year 0, because DateTime starts at year 1
    }
    /// <inheritdoc cref="TotalSeconds(DateTime)"/>
    public static BigDecimal TotalSeconds(this DateTimeOffset DateTimeOffset) {
        return DateTimeOffset.UtcDateTime.TotalSeconds();
    }
    /// <summary>
    /// Returns the date and time's total number of seconds since 0000/00/00 00:00:00.
    /// </summary>
    public static BigDecimal TotalSecondsAt(BigInteger Year, int Month, int Day, int Hour, int Minute, BigDecimal Second) {
        AssertInRange(Month, Day, Hour, Minute, Second);
        return Second
            + Minute * SecondsInMinute
            + Hour * SecondsInHour
            + (DayOfYear(Year, Month, Day) - 1) * SecondsInDay
            + LeapYearsBefore(Year) * SecondsInLeapYear
            + CommonYearsBefore(Year) * SecondsInCommonYear;
    }
    /// <summary>
    /// Throws an exception if any component is outside the valid range.
    /// </summary>
    public static void AssertInRange(int? Month, int? Day, int? Hour, int? Minute, BigDecimal? Second) {
        if (Month is not null && (Month is < 1 or > 12)) {
            throw new ArgumentOutOfRangeException(nameof(Month), "Month must be 1 to 12");
        }
        if (Day is not null && (Day is < 1 or > 31)) {
            throw new ArgumentOutOfRangeException(nameof(Day), "Day must be 1 to 31");
        }
        if (Hour is not null && (Hour is < 0 or > 24)) {
            throw new ArgumentOutOfRangeException(nameof(Hour), "Hour must be 0 to 24");
        }
        if (Minute is not null && (Minute is < 0 or > 60)) {
            throw new ArgumentOutOfRangeException(nameof(Minute), "Minute must be 0 to 60");
        }
        if (Second is not null && (Second < 0 || Second > 60)) {
            throw new ArgumentOutOfRangeException(nameof(Second), "Second must be 0 to 60");
        }
    }
}