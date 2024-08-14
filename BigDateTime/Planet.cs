using System.Numerics;

namespace ExtendedNumerics;

public class Planet {
    public static readonly Planet Earth = new();

    public long SecondsInMinute { get; set; } = 60;
    public long MinutesInHour { get; set; } = 60;
    public long HoursInDay { get; set; } = 24;
    public string[] DaytimeSegments { get; set; } = ["A.M.", "P.M."];

    public long DaysInYear { get; set; } = 365;
    public long ExtraDaysInLeapYear { get; set; } = 1;

    public int DaysInWeek { get; set; } = 7;
    public string[] DaysOfWeek { get; set; } = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    public string[] ShortDaysOfWeek { get; set; } = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    public int MonthsInYear { get; set; } = 12;
    public BigInteger[] DaysInMonths { get; set; } = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    public string[] MonthsOfYear { get; set; } = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    public string[] ShortMonthsOfYear { get; set; } = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    public Func<BigInteger, BigInteger> CountLeapYears { get; set; } = CountLeapYearsOnEarth;
    public Func<BigInteger, BigInteger, BigInteger> GetLeapDaysInMonth { get; set; } = GetLeapDaysInMonthOnEarth;

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
        {"hh", BDTO => (BDTO.Hour % BDTO.Planet.HoursInDaytimeSegment).ToString().PadLeft(2, '0')},
        {"h", BDTO => BDTO.Hour % BDTO.Planet.HoursInDaytimeSegment},
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

    public long SecondsInHour => SecondsInMinute * MinutesInHour;
    public long SecondsInDay => SecondsInMinute * MinutesInHour * HoursInDay;
    public long SecondsInYear => SecondsInMinute * MinutesInHour * HoursInDay * DaysInYear;
    public long SecondsInLeapYear => SecondsInMinute * MinutesInHour * HoursInDay * (DaysInYear + ExtraDaysInLeapYear);
    public BigDecimal HoursInDaytimeSegment => (BigDecimal)HoursInDay / DaytimeSegments.Length;

    public BigInteger GetDaysInMonth(int Month, BigInteger Year) {
        return DaysInMonths[Month - 1] + GetLeapDaysInMonth(Month, Year);
    }
    public string MonthOfYearName(int MonthOfYear, bool Short = false) {
        return Short ? ShortMonthsOfYear[MonthOfYear - 1] : MonthsOfYear[MonthOfYear - 1];
    }
    public string DayOfWeekName(int DayOfWeek, bool Short = false) {
        return Short ? ShortDaysOfWeek[DayOfWeek] : DaysOfWeek[DayOfWeek];
    }
    public string DaytimeSegmentName(int DaytimeSegment) {
        return DaytimeSegments[DaytimeSegment];
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