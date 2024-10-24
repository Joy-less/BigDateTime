using System.Numerics;

namespace BigTime;

public static class EarthConstants {
    public const int SecondsInMinute = 60;
    public const int MinutesInHour = 60;
    public const int HoursInDay = 24;

    public const int SecondsInHour = SecondsInMinute * MinutesInHour;
    public const int SecondsInDay = SecondsInHour * HoursInDay;
    public const int SecondsInCommonYear = SecondsInDay * DaysInCommonYear;
    public const int SecondsInLeapYear = SecondsInDay * DaysInLeapYear;

    public static readonly int[] DaysInMonthInCommonYear = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    public static readonly int[] DaysInMonthInLeapYear = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    public static readonly int[] CumulativeDaysInMonthInCommonYear = [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];
    public static readonly int[] CumulativeDaysInMonthInLeapYear = [0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335];
    public const int DaysInCommonYear = 365;
    public const int DaysInLeapYear = 366;

    public const int DaysIn4Years = DaysInCommonYear * 4 + 1;
    public const int DaysIn100Years = DaysIn4Years * 25 - 1;
    public const int DaysIn400Years = DaysIn100Years * 4 + 1;

    public static readonly string[] DaytimeSegments = ["A.M.", "P.M."];
    public const int HoursInDaytimeSegment = 12;

    public static readonly string[] DaysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    public static readonly string[] ShortDaysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    public const int DaysInWeek = 7;

    public static readonly string[] MonthsOfYear = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    public static readonly string[] ShortMonthsOfYear = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    public const int MonthsInYear = 12;

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
        int[] DaysInMonthInYear = IsLeapYear(Year) ? DaysInMonthInLeapYear : DaysInMonthInCommonYear;
        return DaysInMonthInYear[Month - 1];
    }
    public static int DayOfYear(BigInteger Year, int Month, int Day) {
        int[] CumulativeDaysInMonthInYear = IsLeapYear(Year) ? CumulativeDaysInMonthInLeapYear : CumulativeDaysInMonthInCommonYear;
        return CumulativeDaysInMonthInYear[Month - 1] + Day;
    }
}