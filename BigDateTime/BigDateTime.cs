using System.Numerics;

namespace ExtendedNumerics;

public readonly struct BigDateTime : IComparable, IComparable<BigDateTime> {
    public readonly BigInteger Year;
    public readonly BigInteger Month;
    public readonly BigInteger Day;
    public readonly BigInteger Hour;
    public readonly BigInteger Minute;
    public readonly BigDecimal Second;
    public readonly Planet Planet;

    public BigDateTime(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal Hour, BigDecimal Minute, BigDecimal Second, Planet? Planet = null) {
        this.Year = Year;
        this.Month = Month;
        this.Day = Day;
        this.Hour = Hour.GetWholePart();
        this.Minute = Minute.GetWholePart();
        this.Second = Second;
        this.Planet = Planet ??= Planet.Earth;

        // Add fractional hours to minutes
        this.Minute += (BigInteger)(Hour.GetFractionalPart() / Planet.MinutesInHour);

        // Add fractional minutes to seconds
        this.Second += (BigInteger)(Minute.GetFractionalPart() / Planet.SecondsInMinute);

        // Convert excess seconds to minutes
        this.Minute += (BigInteger)this.Second / Planet.SecondsInMinute;
        this.Second %= Planet.SecondsInMinute;

        // Convert excess minutes to hours
        this.Hour += this.Minute / Planet.MinutesInHour;
        this.Minute %= Planet.MinutesInHour;

        // Convert excess hours to days
        this.Day += this.Hour / Planet.HoursInDay;
        this.Hour %= Planet.HoursInDay;

        // Convert excess months to years
        this.Year += (this.Month - 1) / Planet.MonthsInYear;
        this.Month = ((this.Month - 1) % Planet.MonthsInYear + Planet.MonthsInYear) % Planet.MonthsInYear + 1;

        // Convert excess days to months
        while (true) {
            BigInteger DaysInCurrentMonth = DaysInMonth();
            // Excess positive days
            if (this.Day > DaysInCurrentMonth) {
                this.Day -= DaysInCurrentMonth;
                this.Month++;
            }
            // Excess negative days
            else if (this.Day < -DaysInCurrentMonth) {
                this.Day += DaysInCurrentMonth;
                this.Month--;
            }
            // No excess days
            else {
                break;
            }

            // Convert excess months to years
            this.Year += (this.Month - 1) / Planet.MonthsInYear;
            this.Month = ((this.Month - 1) % Planet.MonthsInYear) + 1;
        }
    }
    public BigDateTime(BigInteger Year, BigInteger Month, BigInteger Day, Planet? Planet = null)
        : this(Year, Month, Day, 0, 0, 0, Planet)
    {
    }
    public BigDateTime(BigDecimal TotalSeconds, Planet? Planet = null)
        : this(0, 1, 1, 0, 0, TotalSeconds, Planet)
    {
    }
    public BigDateTime(DateTime DateTime)
        : this(DateTime.Year, DateTime.Month, DateTime.Day, DateTime.Hour, DateTime.Minute, DateTime.Second + (BigDecimal)DateTime.Millisecond / 1000)
    {
    }

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
        return AddSeconds(Value.TotalSeconds());
    }
    public BigDecimal Subtract(BigDateTime Value) {
        return TotalSeconds() - Value.TotalSeconds();
    }
    public BigDecimal TotalSeconds() {
        return Second
            + Minute * Planet.SecondsInMinute
            + Hour * Planet.SecondsInHour
            + (DayOfYear() - 1) * Planet.SecondsInDay
            + Planet.CountLeapYears.Invoke(Year) * Planet.SecondsInLeapYear
            + (Year - Planet.CountLeapYears.Invoke(Year)) * Planet.SecondsInYear;
    }
    public BigInteger DaysInMonth() {
        return Planet.GetDaysInMonth((int)Month, Year);
    }
    public BigInteger DayOfYear() {
        // Add days passed in current month
        BigInteger DayOfYear = Day;
        // Add days passed in previous months
        for (int CurrentMonth = 1; CurrentMonth < Month; CurrentMonth++) {
            DayOfYear += Planet.GetDaysInMonth(CurrentMonth, Year);
        }
        return DayOfYear;
    }
    public string MonthOfYearName(bool Short = false) {
        return Planet.MonthOfYearName((int)Month, Short);
    }
    public int DayOfWeek() {
        return (int)(TotalSeconds().WholeValue / Planet.SecondsInDay % Planet.DaysInWeek);
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
        return TotalSeconds().CompareTo(Other.TotalSeconds());
    }
    public int CompareTo(object? Other) {
        return Other is BigDateTime OtherBigDateTime ? CompareTo(OtherBigDateTime) : 1;
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
    public static BigDateTime operator +(BigDateTime This, BigDateTime Other) {
        return This.Add(Other);
    }
    public static BigDecimal operator -(BigDateTime This, BigDateTime Other) {
        return This.Subtract(Other);
    }
    public static explicit operator DateTime(BigDateTime BigDateTime) {
        return new DateTime(
            (int)BigDateTime.Year,
            (int)BigDateTime.Month,
            (int)BigDateTime.Day,
            (int)BigDateTime.Hour,
            (int)BigDateTime.Minute,
            (int)BigDateTime.Second,
            (int)(BigDateTime.Second / 1000)
        );
    }
    public static implicit operator BigDateTime(DateTime DateTime) {
        return new BigDateTime(
            DateTime.Year,
            DateTime.Month,
            DateTime.Day,
            DateTime.Hour,
            DateTime.Minute,
            DateTime.Second + (BigDecimal)(DateTime.Millisecond / 1000.0)
        );
    }

    static BigDateTime() {
        // Reduce BigDecimal precision to mitigate slow operations
        BigDecimal.Precision = Math.Min(BigDecimal.Precision, 50);
    }
}