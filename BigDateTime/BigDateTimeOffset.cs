using System.Numerics;

namespace ExtendedNumerics;

public readonly struct BigDateTimeOffset : IComparable, IComparable<BigDateTimeOffset> {
    public readonly BigDateTime BigDateTime;
    /// <summary>
    /// The offset in hours.
    /// </summary>
    public readonly BigDecimal Offset;

    public BigDateTimeOffset(BigDateTime BigDateTime, BigDecimal? Offset = null) {
        this.BigDateTime = BigDateTime;
        this.Offset = Offset ?? BigDecimal.Zero;
    }
    public BigDateTimeOffset(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal Hour, BigDecimal Minute, BigDecimal Second, BigDecimal? Offset = null, Planet? Planet = null)
        : this(new BigDateTime(Year, Month, Day, Hour, Minute, Second, Planet), Offset)
    {
    }
    public BigDateTimeOffset(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal? Offset = null, Planet? Planet = null)
        : this(Year, Month, Day, 0, 0, 0, Offset, Planet)
    {
    }
    public BigDateTimeOffset(BigDecimal TotalSeconds, BigDecimal? Offset = null, Planet? Planet = null)
        : this(0, 1, 1, 0, 0, TotalSeconds, Offset, Planet)
    {
    }
    public BigDateTimeOffset(DateTimeOffset DateTimeOffset)
        : this(DateTimeOffset.Year, DateTimeOffset.Month, DateTimeOffset.Day, DateTimeOffset.Hour, DateTimeOffset.Minute, DateTimeOffset.Second + (BigDecimal)DateTimeOffset.Millisecond / 1000, DateTimeOffset.Offset.TotalHours)
    {
    }

    public BigInteger Year => BigDateTime.Year;
    public BigInteger Month => BigDateTime.Month;
    public BigInteger Day => BigDateTime.Day;
    public BigInteger Hour => BigDateTime.Hour;
    public BigInteger Minute => BigDateTime.Minute;
    public BigDecimal Second => BigDateTime.Second;
    public Planet Planet => BigDateTime.Planet;

    public BigDateTimeOffset AddYears(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddYears(Value), Offset);
    }
    public BigDateTimeOffset AddMonths(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddMonths(Value), Offset);
    }
    public BigDateTimeOffset AddDays(BigInteger Value) {
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
        return AddSeconds(Value / 1000);
    }
    public BigDateTimeOffset AddMicroseconds(BigDecimal Value) {
        return AddMilliseconds(Value / 1000);
    }
    public BigDateTimeOffset AddNanoseconds(BigDecimal Value) {
        return AddMicroseconds(Value / 1000);
    }
    public BigDateTimeOffset Add(BigDateTimeOffset Value) {
        return AddSeconds(Value.TotalSeconds());
    }
    public BigDecimal Subtract(BigDateTimeOffset Value) {
        return TotalSeconds() - Value.TotalSeconds();
    }
    public BigDateTimeOffset WithOffset(BigDecimal Hours) {
        return new BigDateTimeOffset(BigDateTime, Hours);
    }
    public BigDecimal TotalSeconds() {
        return BigDateTime.AddHours(Offset).TotalSeconds();
    }
    public BigInteger DaysInMonth() {
        return BigDateTime.AddHours(Offset).DaysInMonth();
    }
    public BigInteger DayOfYear() {
        return BigDateTime.AddHours(Offset).DayOfYear();
    }
    public string MonthOfYearName(bool Short = false) {
        return BigDateTime.AddHours(Offset).MonthOfYearName(Short);
    }
    public int DayOfWeek() {
        return BigDateTime.AddHours(Offset).DayOfWeek();
    }
    public string DayOfWeekName(bool Short = false) {
        return BigDateTime.AddHours(Offset).DayOfWeekName(Short);
    }
    public int DaytimeSegment() {
        return BigDateTime.AddHours(Offset).DaytimeSegment();
    }
    public string DaytimeSegmentName() {
        return BigDateTime.AddHours(Offset).DaytimeSegmentName();
    }
    public string ToString(string Format) {
        for (int Index = 0; Index < Format.Length; Index++) {
            Redo:
            foreach ((string Find, Func<BigDateTimeOffset, object?> Replacement) in Planet.FormatTable) {
                // Ensure find sequence would fit inside rest of string
                if (Index + Find.Length > Format.Length) {
                    continue;
                }
                // Ensure find sequence follows current index
                if (Format[Index..(Index + Find.Length)] != Find) {
                    continue;
                }
                // Get replacement string
                string ReplacementStr = Replacement(this)?.ToString() ?? "";
                // Replace find sequence
                Format = Format[..Index] + ReplacementStr + Format[(Index + Find.Length)..];
                Index += ReplacementStr.Length;
                // Replace again
                goto Redo;
            }
        }
        return Format;
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
        return TotalSeconds().CompareTo(Other.TotalSeconds());
    }
    public int CompareTo(object? Other) {
        return Other is BigDateTimeOffset OtherBigDateTimeOffset ? CompareTo(OtherBigDateTimeOffset) : 1;
    }

    public static BigDateTimeOffset Parse(string String, Planet? Planet = null) {
        string[] Components = String.Split(['/', ':']);

        return new BigDateTimeOffset(
            Components.ParseBigIntegerOrDefault(0),
            Components.ParseBigIntegerOrDefault(1),
            Components.ParseBigIntegerOrDefault(2),
            Components.ParseBigDecimalOrDefault(3),
            Components.ParseBigDecimalOrDefault(4),
            Components.ParseBigDecimalOrDefault(5),
            Components.ParseBigDecimalOrDefault(6) * (Planet ?? Planet.Earth).SecondsInHour
                + Components.ParseBigDecimalOrDefault(7) * (Planet ?? Planet.Earth).SecondsInMinute
                + Components.ParseBigDecimalOrDefault(8),
            Planet
        );
    }
    public static bool TryParse(string String, out BigDateTimeOffset Result, Planet? Planet = null) {
        try {
            Result = Parse(String, Planet);
            return true;
        }
        catch (Exception) {
            Result = default;
            return false;
        }
    }
    public static BigDateTimeOffset CurrentUniversalTime() {
        return DateTimeOffset.UtcNow;
    }
    public static BigDateTimeOffset CurrentLocalTime() {
        return DateTimeOffset.Now;
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