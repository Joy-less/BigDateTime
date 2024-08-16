using System.Numerics;

namespace ExtendedNumerics;

public class Planet {
    public static readonly Planet Earth = new();

    public BigDecimal SecondsInMinute { get; set; } = 60;
    public BigDecimal MinutesInHour { get; set; } = 60;
    public BigDecimal HoursInDay { get; set; } = 24;
    public string[] DaytimeSegments { get; set; } = ["A.M.", "P.M."];

    public BigDecimal DaysInCommonYear { get; set; } = 365;
    public BigDecimal DaysInLeapYear { get; set; } = 366;

    public BigInteger DaysInWeek { get; set; } = 7;
    public string[] DaysOfWeek { get; set; } = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    public string[] ShortDaysOfWeek { get; set; } = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    public BigInteger MonthsInYear { get; set; } = 12;
    public Func<BigInteger, BigInteger, BigInteger> DaysInMonth { get; set; } = DaysInMonthOnEarth;
    public string[] MonthsOfYear { get; set; } = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    public string[] ShortMonthsOfYear { get; set; } = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    public (BigInteger Years, bool Include)[] LeapYearDivisors = [(4, true), (100, false), (400, true)];

    public Dictionary<string, Func<BigDateTimeOffset, object?>> FormatTable { get; set; } = new() {
        {"dddd", BDTO => BDTO.DayOfWeekName()},
        {"ddd", BDTO => BDTO.DayOfWeekName(true)},
        {"dd", BDTO => BDTO.Day.ToString().PadLeft(2, '0')},
        {"d", BDTO => BDTO.Day},
        {"ffffffff", BDTO => BDTO.Second.GetFractionalPart()},
        {"fffffff", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(7, '0')[..6]},
        {"ffffff", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(6, '0')[..5]},
        {"fffff", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(5, '0')[..4]},
        {"ffff", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(4, '0')[..3]},
        {"fff", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(3, '0')[..2]},
        {"ff", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(2, '0')[..1]},
        {"f", BDTO => BDTO.Second.GetFractionalPart().ToString()[..0]},
        {"FFFFFFFF", BDTO => BDTO.Second.GetFractionalPart().ToString().NullIfZero()},
        {"FFFFFFF", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(7, '0')[..6].NullIfZero()},
        {"FFFFFF", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(6, '0')[..5].NullIfZero()},
        {"FFFFF", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(5, '0')[..4].NullIfZero()},
        {"FFFF", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(4, '0')[..3].NullIfZero()},
        {"FFF", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(3, '0')[..2].NullIfZero()},
        {"FF", BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(2, '0')[..1].NullIfZero()},
        {"F", BDTO => BDTO.Second.GetFractionalPart().ToString()[..0].NullIfZero()},
        {"hh", BDTO => (BDTO.Hour % (BDTO.Planet.HoursInDaytimeSegment + 1)).ToString().PadLeft(2, '0')},
        {"h", BDTO => BDTO.Hour % (BDTO.Planet.HoursInDaytimeSegment + 1)},
        {"HH", BDTO => BDTO.Hour.ToString("D2")},
        {"H", BDTO => BDTO.Hour},
        {"mm", BDTO => BDTO.Minute.ToString("D2")},
        {"m", BDTO => BDTO.Minute},
        {"MMMM", BDTO => BDTO.MonthOfYearName()},
        {"MMM", BDTO => BDTO.MonthOfYearName(true)},
        {"MM", BDTO => BDTO.Month.ToString("D2")},
        {"M", BDTO => BDTO.Month},
        {"ss", BDTO => BDTO.Second.WholeValue.ToString().PadLeft(2, '0')},
        {"s", BDTO => BDTO.Second.WholeValue},
        {"tt", BDTO => BDTO.DaytimeSegmentName()},
        {"t", BDTO => BDTO.DaytimeSegmentName().FirstOrDefault()},
        {"yyyyyy", BDTO => BDTO.Year},
        {"yyyyy", BDTO => BDTO.Year.ToString("D5")},
        {"yyyy", BDTO => BDTO.Year.ToString("D4")},
        {"yyy", BDTO => BDTO.Year.ToString("D3")},
        {"yy", BDTO => string.Concat(BDTO.Year.ToString().TakeLast(2)).PadLeft(2, '0')},
        {"y", BDTO => string.Concat(BDTO.Year.ToString().TakeLast(2))},
        {"zzz", BDTO => (BDTO.Offset.IsNegative() ? "-" : "+") + TimeSpan.FromHours((double)BigDecimal.Abs(BDTO.Offset)).ToString("hh':'mm")},
        {"zz", BDTO => ((BDTO.Offset.IsNegative() ? "-" : "+") + BigDecimal.Abs(BDTO.Offset).WholeValue).PadLeft(2, '0')},
        {"z", BDTO => (BDTO.Offset.IsNegative() ? "-" : "+") + BigDecimal.Abs(BDTO.Offset).WholeValue},
    };

    public BigDecimal SecondsInHour => SecondsInMinute * MinutesInHour;
    public BigDecimal SecondsInDay => SecondsInMinute * MinutesInHour * HoursInDay;
    public BigDecimal SecondsInCommonYear => SecondsInMinute * MinutesInHour * HoursInDay * DaysInCommonYear;
    public BigDecimal SecondsInLeapYear => SecondsInMinute * MinutesInHour * HoursInDay * DaysInLeapYear;
    public BigDecimal HoursInDaytimeSegment => HoursInDay / DaytimeSegments.Length;

    public string MonthOfYearName(int MonthOfYear, bool Short = false) {
        return Short ? ShortMonthsOfYear[MonthOfYear - 1] : MonthsOfYear[MonthOfYear - 1];
    }
    public string DayOfWeekName(int DayOfWeek, bool Short = false) {
        return Short ? ShortDaysOfWeek[DayOfWeek] : DaysOfWeek[DayOfWeek];
    }
    public string DaytimeSegmentName(int DaytimeSegment) {
        return DaytimeSegments[DaytimeSegment];
    }
    public bool IsLeapYear(BigInteger Year) {
        foreach ((BigInteger Divisor, bool Include) in LeapYearDivisors.Reverse()) {
            if (Year % Divisor == 0) {
                return Include;
            }
        }
        return false;
    }
    public BigInteger LeapYearsBefore(BigInteger Year) {
        BigInteger Counter = 0;
        foreach ((BigInteger Divisor, bool Include) in LeapYearDivisors) {
            if (Include) {
                Counter += Year / Divisor;
            }
            else {
                Counter -= Year / Divisor;
            }
        }
        return Counter;
    }
    public BigInteger CommonYearsBefore(BigInteger Year) {
        return Year - LeapYearsBefore(Year);
    }
    public BigDecimal SecondsBeforeYear(BigInteger Year) {
        return (LeapYearsBefore(Year) * SecondsInLeapYear) + (CommonYearsBefore(Year) * SecondsInCommonYear);
    }
    public BigDecimal SecondsInYear(BigInteger Year) {
        return IsLeapYear(Year) ? SecondsInLeapYear : SecondsInCommonYear;
    }
    public BigInteger DayOfYear(BigInteger Year, BigInteger Month, BigInteger Day) {
        // Add days passed in current month
        BigInteger DayOfYear = Day;
        // Add days passed in previous months
        for (int CurrentMonth = 1; CurrentMonth < Month; CurrentMonth++) {
            DayOfYear += DaysInMonth(CurrentMonth, Year);
        }
        return DayOfYear;
    }

    public static BigInteger DaysInMonthOnEarth(BigInteger Month, BigInteger Year) {
        if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12) {
            return 31;
        }
        if (Month == 2) {
            if (IsLeapYearOnEarth(Year)) {
                return 29;
            }
            return 28;
        }
        return 30;
    }
    public static bool IsLeapYearOnEarth(BigInteger Year) {
        return (Year % 400 == 0) || (Year % 4 == 0 && Year % 100 == 0);
    }
}