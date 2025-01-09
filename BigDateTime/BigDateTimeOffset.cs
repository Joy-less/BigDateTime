using System.Globalization;
using System.Numerics;
using ExtendedNumerics;

namespace BigTime;

using static GregorianCalendarConstants;

/// <summary>
/// An arbitrary size and precision DateTimeOffset in the Gregorian calendar.
/// </summary>
public readonly struct BigDateTimeOffset(BigDateTime BigDateTime, BigDecimal Offset = default) : IComparable<BigDateTimeOffset>, IComparable<DateTimeOffset> {
    /// <summary>
    /// The <see cref="BigDateTime"/> component before the offset is applied.
    /// </summary>
    public BigDateTime DateTime { get; } = BigDateTime;
    /// <summary>
    /// The offset from the <see cref="DateTime"/> in hours.
    /// </summary>
    public BigDecimal Offset { get; } = Offset;

    /// <summary>
    /// A <see cref="BigDateTime"/> that represents this <see cref="BigDateTimeOffset"/> with its offset applied.
    /// </summary>
    public BigDateTime Applied { get; } = BigDateTime.AddHours(Offset);
    /// <inheritdoc cref="BigDateTime.TotalSeconds"/>
    public BigDecimal TotalSeconds { get; } = BigDateTime.AddHours(Offset).TotalSeconds;

    /// <summary>
    /// Constructs a <see cref="BigDateTimeOffset"/> from a date, time and offset.
    /// </summary>
    public BigDateTimeOffset(BigInteger Year, int Month, int Day, int Hour, int Minute, BigDecimal Second, BigDecimal Offset = default)
        : this(new BigDateTime(Year, Month, Day, Hour, Minute, Second), Offset) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTimeOffset"/> from a date and offset.
    /// </summary>
    public BigDateTimeOffset(BigInteger Year, int Month, int Day, BigDecimal Offset = default)
        : this(new BigDateTime(Year, Month, Day), Offset) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTimeOffset"/> from a total number of seconds since 0000/00/00 00:00:00 and offset.
    /// </summary>
    public BigDateTimeOffset(BigDecimal TotalSeconds, BigDecimal Offset = default)
        : this(new BigDateTime(TotalSeconds), Offset) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTimeOffset"/> from a less-flexible <see cref="DateTimeOffset"/>.
    /// </summary>
    public BigDateTimeOffset(DateTimeOffset DateTimeOffset)
        : this(new BigDateTime(DateTimeOffset.DateTime), DateTimeOffset.Offset.TotalHours) { }

    /// <inheritdoc cref="BigDateTime.Year"/>
    public BigInteger Year {
        get => Applied.Year;
    }
    /// <inheritdoc cref="BigDateTime.Month"/>
    public BigInteger Month {
        get => Applied.Month;
    }
    /// <inheritdoc cref="BigDateTime.Day"/>
    public BigInteger Day {
        get => Applied.Day;
    }
    /// <inheritdoc cref="BigDateTime.Hour"/>
    public BigInteger Hour {
        get => Applied.Hour;
    }
    /// <inheritdoc cref="BigDateTime.Minute"/>
    public BigInteger Minute {
        get => Applied.Minute;
    }
    /// <inheritdoc cref="BigDateTime.Second"/>
    public BigDecimal Second {
        get => Applied.Second;
    }
    /// <inheritdoc cref="BigDateTime.DayOfYear"/>
    public BigInteger DayOfYear {
        get => Applied.DayOfYear;
    }
    /// <inheritdoc cref="BigDateTime.DayOfWeek"/>
    public DayOfWeek DayOfWeek {
        get => Applied.DayOfWeek;
    }
    /// <inheritdoc cref="BigDateTime.DaytimeSegment"/>
    public DaytimeSegment DaytimeSegment {
        get => Applied.DaytimeSegment;
    }

    /// <inheritdoc cref="BigDateTime.AddYears(BigInteger)"/>
    public BigDateTimeOffset AddYears(BigInteger Value) {
        return new BigDateTimeOffset(DateTime.AddYears(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddMonths(BigInteger)"/>
    public BigDateTimeOffset AddMonths(BigInteger Value) {
        return new BigDateTimeOffset(DateTime.AddMonths(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddDays(BigDecimal)"/>
    public BigDateTimeOffset AddDays(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddDays(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddHours(BigDecimal)"/>
    public BigDateTimeOffset AddHours(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddHours(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddMinutes(BigDecimal)"/>
    public BigDateTimeOffset AddMinutes(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddMinutes(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddSeconds(BigDecimal)"/>
    public BigDateTimeOffset AddSeconds(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddSeconds(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddMilliseconds(BigDecimal)"/>
    public BigDateTimeOffset AddMilliseconds(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddMilliseconds(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddMicroseconds(BigDecimal)"/>
    public BigDateTimeOffset AddMicroseconds(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddMicroseconds(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.AddNanoseconds(BigDecimal)"/>
    public BigDateTimeOffset AddNanoseconds(BigDecimal Value) {
        return new BigDateTimeOffset(DateTime.AddNanoseconds(Value), Offset);
    }
    /// <inheritdoc cref="BigDateTime.Add(BigDateTime)"/>
    public BigDateTimeOffset Add(BigDateTimeOffset Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    /// <inheritdoc cref="BigDateTime.Subtract(BigDateTime)"/>
    public BigDecimal Subtract(BigDateTimeOffset Value) {
        return TotalSeconds - Value.TotalSeconds;
    }
    /// <summary>
    /// Returns a new <see cref="BigDateTimeOffset"/> where the offset is changed to the given number of hours.<br/>
    /// The new <see cref="BigDateTimeOffset"/> will represent a different time.
    /// </summary>
    public BigDateTimeOffset ChangeOffset(BigDecimal Offset) {
        return new BigDateTimeOffset(DateTime, Offset);
    }
    /// <inheritdoc cref="BigDateTime.MonthName(CultureInfo?)"/>
    public string MonthName(CultureInfo? Culture = null) {
        return Applied.MonthName(Culture);
    }
    /// <inheritdoc cref="BigDateTime.AbbreviatedMonthName(CultureInfo?)"/>
    public string AbbreviatedMonthName(CultureInfo? Culture = null) {
        return Applied.AbbreviatedMonthName(Culture);
    }
    /// <inheritdoc cref="BigDateTime.DayOfWeekName(CultureInfo?)"/>
    public string DayOfWeekName(CultureInfo? Culture = null) {
        return Applied.DayOfWeekName(Culture);
    }
    /// <inheritdoc cref="BigDateTime.AbbreviatedDayOfWeekName(CultureInfo?)"/>
    public string AbbreviatedDayOfWeekName(CultureInfo? Culture = null) {
        return Applied.AbbreviatedDayOfWeekName(Culture);
    }
    /// <inheritdoc cref="BigDateTime.DaytimeSegmentName(CultureInfo?)"/>
    public string DaytimeSegmentName(CultureInfo? Culture = null) {
        return Applied.DaytimeSegmentName(Culture);
    }
    /// <inheritdoc cref="BigDateTime.ToString(string)"/>
    public string ToString(string Format) {
        return Formatter.Format(Format, this);
    }
    /// <summary>
    /// Stringifies the date and time like "1970/01/01 00:00:00 +01:00".
    /// </summary>
    public override string ToString() {
        return ToString("yyyy/MM/dd HH:mm:ss zzz");
    }
    /// <summary>
    /// Stringifies the date and time like "Thursday 1 January 1970 00:00:00 +01:00".
    /// </summary>
    public string ToLongString() {
        return ToString("dddd d MMMM yyyy HH:mm:ss zzz");
    }
    /// <summary>
    /// Stringifies the date and time like "Thu 1 Jan 1970 00:00:00 +01:00".
    /// </summary>
    public string ToShortString() {
        return ToString("ddd d MMM yyyy HH:mm:ss zzz");
    }
    /// <summary>
    /// Compares this <see cref="BigDateTime"/> with the <see cref="BigDateTime"/> for sorting.
    /// </summary>
    public int CompareTo(BigDateTimeOffset Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds);
    }
    /// <summary>
    /// Compares this <see cref="BigDateTime"/> with the <see cref="DateTime"/> for sorting.
    /// </summary>
    public int CompareTo(DateTimeOffset Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds());
    }

    /// <summary>
    /// Parses a <see cref="BigDateTimeOffset"/> from the string or throws an exception.
    /// </summary>
    /// <remarks>Currently only parses strings in the format "y/M/d H:m:s +H:m:s", where each component is optional and each separator is interchangeable.</remarks>
    public static BigDateTimeOffset Parse(ReadOnlySpan<char> String) {
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
    /// <summary>
    /// Parses a <see cref="BigDateTimeOffset"/> from the string or returns false.
    /// </summary>
    /// <remarks>Currently only parses strings in the format "y/M/d H:m:s +H:m:s", where each component is optional and each separator is interchangeable.</remarks>
    public static bool TryParse(ReadOnlySpan<char> String, out BigDateTimeOffset Result) {
        try {
            Result = Parse(String);
            return true;
        }
        catch (Exception) {
            Result = default;
            return false;
        }
    }
    /// <summary>
    /// Returns a <see cref="BigDateTimeOffset"/> representing the current time at the given UTC offset.
    /// </summary>
    public static BigDateTimeOffset Now(BigDecimal Offset) {
        return new BigDateTimeOffset(System.DateTime.UtcNow, Offset);
    }
    /// <summary>
    /// Returns a <see cref="BigDateTimeOffset"/> representing the current time at the local UTC offset.
    /// </summary>
    public static BigDateTimeOffset Now() {
        return Now(DateTimeOffset.Now.Offset.TotalHours);
    }
    /// <inheritdoc cref="Add(BigDateTimeOffset)"/>
    public static BigDateTimeOffset operator +(BigDateTimeOffset This, BigDateTimeOffset Other) {
        return This.Add(Other);
    }
    /// <inheritdoc cref="Subtract(BigDateTimeOffset)"/>
    public static BigDecimal operator -(BigDateTimeOffset This, BigDateTimeOffset Other) {
        return This.Subtract(Other);
    }
    /// <summary>
    /// Converts the <see cref="DateTimeOffset"/> to a <see cref="BigDateTimeOffset"/>.
    /// </summary>
    public static implicit operator BigDateTimeOffset(DateTimeOffset DateTimeOffset) {
        return new BigDateTimeOffset(DateTimeOffset);
    }
    /// <summary>
    /// Converts the <see cref="BigDateTimeOffset"/> to a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <remarks>
    /// If <see cref="Second"/> is very precise, it loses precision.<br/>
    /// If <see cref="Year"/> is outside 1 to 9999, an exception is thrown.<br/>
    /// If <see cref="Offset"/> is outside the valid range or precision of a <see cref="double"/>, an exception is thrown or it loses precision.
    /// </remarks>
    public static explicit operator DateTimeOffset(BigDateTimeOffset BigDateTimeOffset) {
        return new DateTimeOffset((DateTime)BigDateTimeOffset.DateTime, TimeSpan.FromHours((double)BigDateTimeOffset.Offset));
    }
}