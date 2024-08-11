using System.Numerics;

namespace BigDate;

public class BigDatePlanet {
    public static readonly BigDatePlanet Default = new();

    public long SecondsInMinute = 60;
    public long MinutesInHour = 60;
    public long HoursInDay = 24;

    public long DaysInYear = 365;
    public long DaysInLeapYear = 366;

    public int DaysInWeek = 7;
    public string[] DaysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

    public int MonthsInYear = 12;
    public BigInteger[] DaysInMonths = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    public string[] MonthsOfYear = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    public Func<BigInteger, BigInteger> CountLeapYears = CountLeapYearsOnEarth;
    public Func<BigInteger, BigInteger, BigInteger> GetLeapDaysInMonth = GetLeapDaysInMonthOnEarth;

    public long SecondsInHour => SecondsInMinute * MinutesInHour;
    public long SecondsInDay => SecondsInMinute * MinutesInHour * HoursInDay;
    public long SecondsInYear => SecondsInMinute * MinutesInHour * HoursInDay * DaysInYear;
    public long SecondsInLeapYear => SecondsInMinute * MinutesInHour * HoursInDay * DaysInLeapYear;

    public BigInteger GetDaysInMonth(int Month, BigInteger Year) {
        return DaysInMonths[Month - 1] + GetLeapDaysInMonth(Month, Year);
    }
    public string MonthOfYearName(int MonthOfYear, int Limit = int.MaxValue) {
        string Name = MonthsOfYear[MonthOfYear - 1];
        return (Name.Length > Limit) ? Name[..Limit] : Name;
    }
    public string DayOfWeekName(int DayOfWeek, int Limit = int.MaxValue) {
        string Name = DaysOfWeek[DayOfWeek - 1];
        return (Name.Length > Limit) ? Name[..Limit] : Name;
    }
    public BigInteger CountLeapYearsBetween(BigInteger FirstYear, BigInteger LastYear) {
        return CountLeapYears(LastYear) - CountLeapYears(FirstYear - 1);
    }
    public BigInteger CountNonLeapYearsBetween(BigInteger FirstYear, BigInteger EndYear) {
        return EndYear - FirstYear - CountLeapYearsBetween(FirstYear, EndYear);
    }

    public static bool IsLeapYearOnEarth(BigInteger Year) {
        return (Year % 400 == 0) || (Year % 4 == 0 && Year % 100 == 0);
    }
    public static BigInteger CountLeapYearsOnEarth(BigInteger Year) {
        return (Year / 4) - (Year / 100) + (Year / 400);
    }
    public static BigInteger GetLeapDaysInMonthOnEarth(BigInteger Month, BigInteger Year) {
        if (Month == 2 && IsLeapYearOnEarth(Year)) {
            return 1;
        }
        return 0;
    }
}