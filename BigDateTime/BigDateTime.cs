using System.Numerics;

namespace ExtendedNumerics;

public readonly struct BigDateTime(BigDecimal TotalSeconds, Planet? Planet = null) : IComparable, IComparable<BigDateTime> {
    public readonly BigDecimal TotalSeconds = TotalSeconds;
    public readonly Planet Planet = Planet ?? Planet.Earth;

    public BigDateTime(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal Hour, BigDecimal Minute, BigDecimal Second, Planet? Planet = null)
        : this(CalculateTotalSeconds(Year, Month, Day, Hour, Minute, Second, Planet), Planet) { }
    public BigDateTime(BigInteger Year, BigInteger Month, BigInteger Day, Planet? Planet = null)
        : this(Year, Month, Day, 0, 0, 0, Planet) { }
    public BigDateTime(DateTime DateTime)
        : this(DateTime.Subtract(DateTime.MinValue).TotalSeconds + Planet.Earth.SecondsInCommonYear) { }

    public BigDateTime AddYears(BigInteger Value) {
        return new BigDateTime(Year + Value, Month, Day, Hour, Minute, Second);
    }
    public BigDateTime AddMonths(BigInteger Value) {
        return new BigDateTime(Year, Month + Value, Day, Hour, Minute, Second);
    }
    public BigDateTime AddDays(BigInteger Value) {
        return new BigDateTime(Year, Month, Day + Value, Hour, Minute, Second);
    }
    public BigDateTime AddHours(BigDecimal Value) {
        return new BigDateTime(Year, Month, Day, Hour + Value, Minute, Second);
    }
    public BigDateTime AddMinutes(BigDecimal Value) {
        return new BigDateTime(Year, Month, Day, Hour, Minute + Value, Second);
    }
    public BigDateTime AddSeconds(BigDecimal Value) {
        return new BigDateTime(Year, Month, Day, Hour, Minute, Second + Value);
    }
    public BigDateTime AddMilliseconds(BigDecimal Value) {
        return AddSeconds(Value / 1000);
    }
    public BigDateTime AddMicroseconds(BigDecimal Value) {
        return AddMilliseconds(Value / 1000);
    }
    public BigDateTime AddNanoseconds(BigDecimal Value) {
        return AddMicroseconds(Value / 1000);
    }
    public BigDateTime Add(BigDateTime Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    public BigDecimal Subtract(BigDateTime Value) {
        return TotalSeconds - Value.TotalSeconds;
    }
    public BigInteger DaysInMonth() {
        return Planet.DaysInMonth((int)Month, Year);
    }
    public BigInteger DayOfYear() {
        return Planet.DayOfYear(Year, Month, Day);
    }
    public string MonthOfYearName(bool Short = false) {
        return Planet.MonthOfYearName((int)Month, Short);
    }
    public int DayOfWeek() {
        return (int)(TotalSeconds / Planet.SecondsInDay % Planet.DaysInWeek).WholeValue;
    }
    public string DayOfWeekName(bool Short = false) {
        return Planet.DayOfWeekName(DayOfWeek(), Short);
    }
    public int DaytimeSegment() {
        return (int)(Hour / Planet.HoursInDaytimeSegment).WholeValue;
    }
    public string DaytimeSegmentName() {
        return Planet.DaytimeSegmentName(DaytimeSegment());
    }
    /// <summary>
    /// Stringifies the BigDateTime using a format string.
    /// </summary>
    public string ToString(string Format) {
        return new BigDateTimeOffset(this).ToString(Format);
    }
    /// <summary>
    /// Stringifies the BigDateTime like "1970/01/01 00:00:00".
    /// </summary>
    public override string ToString() {
        return ToString("yyyy/MM/dd HH:mm:ss");
    }
    /// <summary>
    /// Stringifies the BigDateTime like "Thursday 1 January 1970 00:00:00".
    /// </summary>
    public string ToLongString() {
        return ToString("dddd d MMMM yyyy HH:mm:ss");
    }
    /// <summary>
    /// Stringifies the BigDateTime like "Thu 1 Jan 1970 00:00:00".
    /// </summary>
    public string ToShortString() {
        return ToString("ddd d MMM yyyy HH:mm:ss");
    }
    public int CompareTo(BigDateTime Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds);
    }
    public int CompareTo(object? Other) {
        return Other is BigDateTime OtherBigDateTime ? CompareTo(OtherBigDateTime) : 1;
    }

    public BigInteger Year {
        get {
            // Find year above current seconds
            BigInteger Counter = 0;
            while (true) {
                BigDecimal SecondsBeforeNextYear = Planet.SecondsBeforeYear(Counter + 1);
                if (TotalSeconds < SecondsBeforeNextYear) {
                    break;
                }
                Counter++;
            }
            return Counter;
        }
    }
    public BigInteger Month {
        get {
            BigInteger Year = this.Year;
            BigDecimal RemainingSeconds = TotalSeconds;

            // Subtract seconds up to start of year
            RemainingSeconds -= Planet.SecondsBeforeYear(Year);

            // Calculate current month within this year
            BigInteger Counter = 1;
            while (true) {
                BigDecimal SecondsInMonth = Planet.DaysInMonth(Counter, Year) * Planet.SecondsInDay;
                if (RemainingSeconds < SecondsInMonth) {
                    break;
                }
                RemainingSeconds -= SecondsInMonth;
                Counter++;
            }
            return Counter;
        }
    }
    public BigInteger Day {
        get {
            BigInteger Year = this.Year;
            BigInteger Month = this.Month;

            // Calculate total seconds before start of month
            BigDecimal SecondsBeforeThisMonth = 0;
            for (BigInteger Counter = 1; Counter < Month; Counter++) {
                SecondsBeforeThisMonth += Planet.DaysInMonth(Counter, Year) * Planet.SecondsInDay;
            }

            // Calculate number of seconds in this month
            BigDecimal SecondsInThisMonth = TotalSeconds - (Planet.SecondsBeforeYear(Year) + SecondsBeforeThisMonth);

            // Calculate day from remaining seconds
            return (SecondsInThisMonth / Planet.SecondsInDay).WholeValue + 1;
        }
    }
    public BigInteger Hour {
        get {
            return (TotalSeconds % Planet.SecondsInDay / Planet.SecondsInHour).WholeValue;
        }
    }
    public BigInteger Minute {
        get {
            return (TotalSeconds % Planet.SecondsInHour / Planet.SecondsInMinute).WholeValue;
        }
    }
    public BigDecimal Second {
        get {
            return TotalSeconds % Planet.SecondsInMinute;
        }
    }

    public static BigDateTime Parse(string String, Planet? Planet = null) {
        string[] Components = String.Split(['/', ':']);

        return new BigDateTime(
            Components.ParseBigIntegerOrDefault(0),
            Components.ParseBigIntegerOrDefault(1),
            Components.ParseBigIntegerOrDefault(2),
            Components.ParseBigDecimalOrDefault(3),
            Components.ParseBigDecimalOrDefault(4),
            Components.ParseBigDecimalOrDefault(5),
            Planet
        );
    }
    public static bool TryParse(string String, out BigDateTime Result, Planet? Planet = null) {
        try {
            Result = Parse(String, Planet);
            return true;
        }
        catch (Exception) {
            Result = default;
            return false;
        }
    }
    public static BigDateTime CurrentUniversalTime() {
        return DateTime.UtcNow;
    }
    public static BigDateTime CurrentLocalTime() {
        return DateTime.Now;
    }
    public static BigDecimal CalculateTotalSeconds(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal Hour, BigDecimal Minute, BigDecimal Second, Planet? Planet = null) {
        Planet ??= Planet.Earth;
        return Second
            + Minute * Planet.SecondsInMinute
            + Hour * Planet.SecondsInHour
            + (Planet.DayOfYear(Year, Month, Day) - 1) * Planet.SecondsInDay
            + Planet.LeapYearsBefore(Year) * Planet.SecondsInLeapYear
            + (Year - Planet.LeapYearsBefore(Year)) * Planet.SecondsInCommonYear;
    }
    public static BigDateTime operator +(BigDateTime This, BigDateTime Other) {
        return This.Add(Other);
    }
    public static BigDecimal operator -(BigDateTime This, BigDateTime Other) {
        return This.Subtract(Other);
    }
    public static explicit operator DateTime(BigDateTime BigDateTime) {
        return new DateTime(TimeSpan.FromSeconds((double)BigDateTime.TotalSeconds).Ticks);
    }
    public static implicit operator BigDateTime(DateTime DateTime) {
        return new BigDateTime(DateTime);
    }

    static BigDateTime() {
        // Reduce BigDecimal precision to mitigate slow operations
        BigDecimal.Precision = Math.Min(BigDecimal.Precision, 50);
    }
}