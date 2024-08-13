using System.Numerics;

namespace ExtendedNumerics;

[Serializable]
public readonly struct BigDateTime : IComparable, IComparable<BigDateTime> {
    public readonly BigInteger Year;
    public readonly BigInteger Month;
    public readonly BigInteger Day;
    public readonly BigInteger Hour;
    public readonly BigInteger Minute;
    public readonly BigDecimal Second;
    public readonly BigDatePlanet Planet;

    public BigDateTime(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal Hour, BigDecimal Minute, BigDecimal Second, BigDatePlanet? Planet = null) {
        this.Year = Year;
        this.Month = Month;
        this.Day = Day;
        this.Hour = Hour.GetWholePart();
        this.Minute = Minute.GetWholePart();
        this.Second = Second;
        this.Planet = Planet ??= BigDatePlanet.Default;

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
    public BigDateTime(BigInteger Year, BigInteger Month, BigInteger Day, BigDatePlanet? Planet = null)
        : this(Year, Month, Day, 0, 0, 0, Planet)
    {
    }
    public BigDateTime(BigDecimal TotalSeconds, BigDatePlanet? Planet = null)
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
        BigInteger DayInYear = Day;
        for (int CurrentMonth = 1; CurrentMonth < Month; CurrentMonth++) {
            DayInYear += Planet.GetDaysInMonth(CurrentMonth, Year);
        }
        return DayInYear;
    }
    public BigInteger DayOfWeek() {
        return (BigInteger)TotalSeconds() / Planet.SecondsInDay % 7 + 1;
    }
    public string ToString(string Format) {
        return Format
            .Format('y', Year)
            .Format('M', Month)
            .Format('d', Day)
            .Format('h', Hour)
            .Format('m', Minute)
            .Format('s', Second.GetWholePart())
            .Format('f', Second.GetFractionalPart());
    }
    public override string ToString() {
        // e.g. "1970/01/01 00:00:00"
        return ToString("yyyy/MM/dd hh:mm:ss");
    }
    public string ToLongString() {
        // e.g. "Thursday 1 January 1970 00:00:00"
        return Planet.DayOfWeekName((int)DayOfWeek()) + ToString(" dd ") + Planet.MonthOfYearName((int)Month) + ToString(" yyyy hh:mm:ss");
    }
    public string ToShortString() {
        // e.g. "Thu 1 Jan 1970 00:00:00"
        return Planet.DayOfWeekName((int)DayOfWeek(), 3) + ToString(" dd ") + Planet.MonthOfYearName((int)Month, 3) + ToString(" yyyy hh:mm:ss");
    }
    public int CompareTo(BigDateTime Other) {
        return TotalSeconds().CompareTo(Other.TotalSeconds());
    }
    public int CompareTo(object? Other) {
        return Other is BigDateTime OtherBigDateTime ? CompareTo(OtherBigDateTime) : 1;
    }

    public static BigDateTime Parse(string String, BigDatePlanet? Planet = null) {
        List<BigDecimal> Components = String.ParseDateComponents(6);
        return new BigDateTime(
            Components[0].WholeValue,
            Components[1].WholeValue,
            Components[2].WholeValue,
            Components[3],
            Components[4],
            Components[5],
            Planet
        );
    }
    public static bool TryParse(string String, out BigDateTime Result, BigDatePlanet? Planet = null) {
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