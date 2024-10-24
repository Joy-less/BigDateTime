using System.Numerics;
using ExtendedNumerics;

namespace BigTime;

using static EarthConstants;
using static Extensions;

/// <summary>
/// An arbitrary precision DateTime.
/// </summary>
public readonly struct BigDateTime(BigDecimal TotalSeconds) : IComparable, IComparable<BigDateTime> {
    /// <summary>
    /// The total number of seconds since 0000/00/00 00:00:00.
    /// </summary>
    public readonly BigDecimal TotalSeconds = TotalSeconds;

    /// <summary>
    /// Constructs a <see cref="BigDateTime"/> from a date and time.
    /// </summary>
    public BigDateTime(BigInteger Year, int Month, int Day, int Hour, int Minute, BigDecimal Second)
        : this(TotalSecondsAt(Year, Month, Day, Hour, Minute, Second)) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTime"/> from a date.
    /// </summary>
    public BigDateTime(BigInteger Year, int Month, int Day)
        : this(Year, Month, Day, 0, 0, 0) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTime"/> from a <see cref="DateTime"/>.
    /// </summary>
    public BigDateTime(DateTime DateTime)
        : this(DateTime.TotalSeconds()) { }

    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the years added.
    /// </summary>
    public BigDateTime AddYears(BigInteger Value) {
        return AddMonths(Value * 12);
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the months added.
    /// </summary>
    public BigDateTime AddMonths(BigInteger Value) {
        return new BigDateTime(BlackMagic.AddMonths(TotalSeconds, Value));
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the days added.
    /// </summary>
    public BigDateTime AddDays(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInDay));
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the hours added.
    /// </summary>
    public BigDateTime AddHours(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInHour));
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the minutes added.
    /// </summary>
    public BigDateTime AddMinutes(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInMinute));
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the seconds added.
    /// </summary>
    public BigDateTime AddSeconds(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + Value);
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the milliseconds added.
    /// </summary>
    public BigDateTime AddMilliseconds(BigDecimal Value) {
        return AddSeconds(Value / 1000);
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the microseconds added.
    /// </summary>
    public BigDateTime AddMicroseconds(BigDecimal Value) {
        return AddMilliseconds(Value / 1000);
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTime"/> with the nanoseconds added.
    /// </summary>
    public BigDateTime AddNanoseconds(BigDecimal Value) {
        return AddMicroseconds(Value / 1000);
    }
    public BigDateTime Add(BigDateTime Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    public BigDecimal Subtract(BigDateTime Value) {
        return TotalSeconds - Value.TotalSeconds;
    }
    public string MonthOfYearName(bool Short = false) {
        return Short ? ShortMonthsOfYear[Month - 1] : MonthsOfYear[Month - 1];
    }
    public string DayOfWeekName(bool Short = false) {
        return Short ? ShortDaysOfWeek[(int)DayOfWeek] : DaysOfWeek[(int)DayOfWeek];
    }
    public string DaytimeSegmentName() {
        return DaytimeSegments[DaytimeSegment];
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
        get => BlackMagic.GetYear(TotalSeconds.WholeValue);
    }
    public int Month {
        get => BlackMagic.GetMonthOfYear(TotalSeconds.WholeValue);
    }
    public int Day {
        get => BlackMagic.GetDayOfMonth(TotalSeconds.WholeValue);
    }
    public int Hour {
        get => (int)(TotalSeconds % SecondsInDay / SecondsInHour).WholeValue;
    }
    public int Minute {
        get => (int)(TotalSeconds % SecondsInHour / SecondsInMinute).WholeValue;
    }
    public BigDecimal Second {
        get => TotalSeconds % SecondsInMinute;
    }
    public int DayOfYear {
        get => BlackMagic.GetDayOfYear(TotalSeconds.WholeValue);
    }
    public DayOfWeek DayOfWeek {
        get => (DayOfWeek)(int)(TotalSeconds / SecondsInDay % DaysInWeek).WholeValue;
    }
    public int DaytimeSegment {
        get => Hour / HoursInDaytimeSegment;
    }

    public static BigDateTime Parse(string String) {
        Parser Parser = new(String);

        Parser.EatBigInteger(out BigInteger Year);
        Parser.EatInt(out int Month, 1);
        Parser.EatInt(out int Day, 1);
        Parser.EatInt(out int Hour);
        Parser.EatInt(out int Minute);
        Parser.EatBigDecimal(out BigDecimal Second);

        return new BigDateTime(Year, Month, Day, Hour, Minute, Second);
    }
    public static bool TryParse(string String, out BigDateTime Result) {
        try {
            Result = Parse(String);
            return true;
        }
        catch (Exception) {
            Result = default;
            return false;
        }
    }
    public static BigDateTime Now(BigDecimal Offset) {
        return new BigDateTime(DateTime.UtcNow).AddHours(Offset);
    }
    public static BigDateTime Now() {
        return Now(DateTimeOffset.Now.Offset.TotalHours);
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
}