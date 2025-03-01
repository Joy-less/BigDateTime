using System.Numerics;

namespace ExtendedNumerics;

/// <summary>
/// Date and time constants that apply to the Gregorian calendar.
/// </summary>
internal static class GregorianCalendarConstants {
    public const int SecondsInMinute = 60;
    public const int MinutesInHour = 60;
    public const int HoursInDay = 24;
    public const int DaysInCommonYear = 365;
    public const int DaysInLeapYear = 366;
    public const int DaysInWeek = 7;
    public const int MonthsInYear = 12;
    public const int HoursInDaytimeSegment = 12;

    public const int SecondsInHour = SecondsInMinute * MinutesInHour;
    public const int SecondsInDay = SecondsInHour * HoursInDay;
    public const int SecondsInCommonYear = SecondsInDay * DaysInCommonYear;
    public const int SecondsInLeapYear = SecondsInDay * DaysInLeapYear;

    public const int DaysIn4Years = DaysInCommonYear * 4 + 1;
    public const int DaysIn100Years = DaysIn4Years * 25 - 1;
    public const int DaysIn400Years = DaysIn100Years * 4 + 1;

    public static ReadOnlySpan<int> DaysInMonthInCommonYear => [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    public static ReadOnlySpan<int> DaysInMonthInLeapYear => [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    public static ReadOnlySpan<int> DaysUpToMonthInCommonYear => [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];
    public static ReadOnlySpan<int> DaysUpToMonthInLeapYear => [0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335];

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
    public static int DaysInMonth(int Month, BigInteger Year) {
        ReadOnlySpan<int> DaysInMonthInYear = IsLeapYear(Year) ? DaysInMonthInLeapYear : DaysInMonthInCommonYear;
        return DaysInMonthInYear[Month - 1];
    }
    public static int DayOfYear(BigInteger Year, int Month, int Day) {
        ReadOnlySpan<int> DaysUpToMonthInYear = IsLeapYear(Year) ? DaysUpToMonthInLeapYear : DaysUpToMonthInCommonYear;
        return DaysUpToMonthInYear[Month - 1] + Day;
    }
}