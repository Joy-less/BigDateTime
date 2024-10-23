using System.Numerics;
using ExtendedNumerics;

namespace BigTime;

using static EarthConstants;

internal static class BigDateTimeExtensions {
    public static string? NullIfZero(this string Number) {
        return BigDecimal.Parse(Number).IsZero() ? null : Number;
    }
    public static int DaysInMonth(int Month, BigInteger Year) {
        int[] DaysInMonthInYear = IsLeapYear(Year) ? DaysInMonthInLeapYear : DaysInMonthInCommonYear;
        return DaysInMonthInYear[Month - 1];
    }
    public static int DayOfYear(BigInteger Year, int Month, int Day) {
        int[] CumulativeDaysInMonthInYear = IsLeapYear(Year) ? CumulativeDaysInMonthInLeapYear : CumulativeDaysInMonthInCommonYear;
        return CumulativeDaysInMonthInYear[Month - 1] + Day;
    }
    public static bool IsLeapYear(BigInteger Year) {
        return Year % 4 == 0 && (Year % 100 != 0 || Year % 400 == 0);
    }
    public static BigInteger LeapYearsBefore(BigInteger Year) {
        Year -= 1;
        return (Year / 4) - (Year / 100) + (Year / 400);
    }
    public static BigInteger CommonYearsBefore(BigInteger Year) {
        return Year - LeapYearsBefore(Year);
    }
    public static BigDecimal TotalSecondsAt(BigInteger Year, int Month, int Day, int Hour, int Minute, BigDecimal Second) {
        AssertInRange(Month, Day, Hour, Minute, Second);
        return Second
            + Minute * SecondsInMinute
            + Hour * SecondsInHour
            + (DayOfYear(Year, Month, Day) - 1) * SecondsInDay
            + LeapYearsBefore(Year) * SecondsInLeapYear
            + CommonYearsBefore(Year) * SecondsInCommonYear;
    }
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