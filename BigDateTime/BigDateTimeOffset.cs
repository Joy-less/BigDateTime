using System.Numerics;
using ExtendedNumerics;

namespace BigTime;

using static EarthConstants;

public readonly struct BigDateTimeOffset(BigDateTime BigDateTime, BigDecimal Offset = default) : IComparable, IComparable<BigDateTimeOffset> {
    public readonly BigDateTime BigDateTime = BigDateTime;
    /// <summary>
    /// The offset in hours.
    /// </summary>
    public readonly BigDecimal Offset = Offset;

    public readonly BigDecimal TotalSeconds = BigDateTime.AddHours(Offset).TotalSeconds;

    public BigDateTimeOffset(BigInteger Year, int Month, int Day, int Hour, int Minute, BigDecimal Second, BigDecimal Offset = default)
        : this(new BigDateTime(Year, Month, Day, Hour, Minute, Second), Offset) { }
    public BigDateTimeOffset(BigInteger Year, int Month, int Day, BigDecimal Offset = default)
        : this(new BigDateTime(Year, Month, Day), Offset) { }
    public BigDateTimeOffset(BigDecimal TotalSeconds, BigDecimal Offset = default)
        : this(new BigDateTime(TotalSeconds), Offset) { }
    public BigDateTimeOffset(DateTimeOffset DateTimeOffset)
        : this(new BigDateTime(DateTimeOffset.DateTime), DateTimeOffset.Offset.TotalHours) { }

    public BigInteger Year => ApplyOffset().Year;
    public BigInteger Month => ApplyOffset().Month;
    public BigInteger Day => ApplyOffset().Day;
    public BigInteger Hour => ApplyOffset().Hour;
    public BigInteger Minute => ApplyOffset().Minute;
    public BigDecimal Second => ApplyOffset().Second;
    public BigInteger DayOfYear => ApplyOffset().DayOfYear;
    public DayOfWeek DayOfWeek => ApplyOffset().DayOfWeek;
    public int DaytimeSegment => ApplyOffset().DaytimeSegment;

    public BigDateTimeOffset AddYears(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddYears(Value), Offset);
    }
    public BigDateTimeOffset AddMonths(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddMonths(Value), Offset);
    }
    public BigDateTimeOffset AddDays(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddDays(Value), Offset);
    }
    public BigDateTimeOffset AddHours(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddHours(Value), Offset);
    }
    public BigDateTimeOffset AddMinutes(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddMinutes(Value), Offset);
    }
    public BigDateTimeOffset AddSeconds(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddSeconds(Value), Offset);
    }
    public BigDateTimeOffset AddMilliseconds(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddMilliseconds(Value), Offset);
    }
    public BigDateTimeOffset AddMicroseconds(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddMicroseconds(Value), Offset);
    }
    public BigDateTimeOffset AddNanoseconds(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddNanoseconds(Value), Offset);
    }
    public BigDateTimeOffset Add(BigDateTimeOffset Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    public BigDecimal Subtract(BigDateTimeOffset Value) {
        return TotalSeconds - Value.TotalSeconds;
    }
    public BigDateTimeOffset WithOffset(BigDecimal Hours) {
        return new BigDateTimeOffset(BigDateTime, Hours);
    }
    public string MonthOfYearName(bool Short = false) {
        return ApplyOffset().MonthOfYearName(Short);
    }
    public string DayOfWeekName(bool Short = false) {
        return ApplyOffset().DayOfWeekName(Short);
    }
    public string DaytimeSegmentName() {
        return ApplyOffset().DaytimeSegmentName();
    }
    public BigDateTime ApplyOffset() {
        return BigDateTime.AddHours(Offset);
    }
    public string ToString(string Format) {
        return Formatter.Format(Format, this);
    }
    /// <summary>
    /// Stringifies the BigDateTimeOffset like "1970/01/01 00:00:00 +01:00".
    /// </summary>
    public override string ToString() {
        return ToString("yyyy/MM/dd HH:mm:ss zzz");
    }
    /// <summary>
    /// Stringifies the BigDateTimeOffset like "Thursday 1 January 1970 00:00:00 +01:00".
    /// </summary>
    public string ToLongString() {
        return ToString("dddd d MMMM yyyy HH:mm:ss zzz");
    }
    /// <summary>
    /// Stringifies the BigDateTimeOffset like "Thu 1 Jan 1970 00:00:00 +01:00".
    /// </summary>
    public string ToShortString() {
        return ToString("ddd d MMM yyyy HH:mm:ss zzz");
    }
    public int CompareTo(BigDateTimeOffset Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds);
    }
    public int CompareTo(object? Other) {
        return Other is BigDateTimeOffset OtherBigDateTimeOffset ? CompareTo(OtherBigDateTimeOffset) : 1;
    }

    public static BigDateTimeOffset Parse(string String) {
        Parser Parser = new(String);

        Parser.EatBigInteger(out BigInteger Year);
        Parser.EatInt(out int Month, 1);
        Parser.EatInt(out int Day, 1);
        Parser.EatInt(out int Hour);
        Parser.EatInt(out int Minute);
        Parser.EatBigDecimal(out BigDecimal Second);
        Parser.EatBigDecimal(out BigDecimal OffsetHours);
        Parser.EatBigDecimal(out BigDecimal OffsetMinutes);
        Parser.EatBigDecimal(out BigDecimal OffsetSeconds);

        return new BigDateTimeOffset(Year, Month, Day, Hour, Minute, Second, (OffsetHours * SecondsInHour) + (OffsetMinutes * SecondsInMinute) + OffsetSeconds);
    }
    public static bool TryParse(string String, out BigDateTimeOffset Result) {
        try {
            Result = Parse(String);
            return true;
        }
        catch (Exception) {
            Result = default;
            return false;
        }
    }
    public static BigDateTimeOffset Now(BigDecimal Offset) {
        return new BigDateTimeOffset(DateTime.UtcNow, Offset);
    }
    public static BigDateTimeOffset Now() {
        return Now(DateTimeOffset.Now.Offset.TotalHours);
    }
    public static BigDateTimeOffset operator +(BigDateTimeOffset This, BigDateTimeOffset Other) {
        return This.Add(Other);
    }
    public static BigDecimal operator -(BigDateTimeOffset This, BigDateTimeOffset Other) {
        return This.Subtract(Other);
    }
    public static explicit operator DateTimeOffset(BigDateTimeOffset BigDateTimeOffset) {
        return new DateTimeOffset((DateTime)BigDateTimeOffset.BigDateTime, TimeSpan.FromHours((double)BigDateTimeOffset.Offset));
    }
    public static implicit operator BigDateTimeOffset(DateTimeOffset DateTimeOffset) {
        return new BigDateTimeOffset(DateTimeOffset);
    }
}