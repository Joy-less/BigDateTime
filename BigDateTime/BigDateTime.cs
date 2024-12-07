using System.Globalization;
using System.Numerics;
using ExtendedNumerics;

namespace BigTime;

using static GregorianCalendarConstants;
using static Extensions;

/// <summary>
/// An arbitrary size and precision DateTime in the Gregorian calendar.
/// </summary>
public readonly struct BigDateTime(BigDecimal TotalSeconds) : IComparable<BigDateTime>, IComparable<DateTime> {
    /// <summary>
    /// The total number of seconds since 0000/00/00 00:00:00.
    /// </summary>
    public BigDecimal TotalSeconds { get; } = TotalSeconds;

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
    /// Constructs a <see cref="BigDateTime"/> from a less-flexible <see cref="DateTime"/>.
    /// </summary>
    public BigDateTime(DateTime DateTime)
        : this(DateTime.TotalSeconds()) { }

    /// <summary>
    /// Adds the number of years.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public BigDateTime AddYears(BigInteger Value) {
        return AddMonths(Value * 12);
    }
    /// <summary>
    /// Adds the number of months.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public BigDateTime AddMonths(BigInteger Value) {
        return new BigDateTime(BlackMagic.AddMonths(TotalSeconds, Value));
    }
    /// <summary>
    /// Adds the number of days.
    /// </summary>
    public BigDateTime AddDays(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInDay));
    }
    /// <summary>
    /// Adds the number of hours.
    /// </summary>
    public BigDateTime AddHours(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInHour));
    }
    /// <summary>
    /// Adds the number of minutes.
    /// </summary>
    public BigDateTime AddMinutes(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInMinute));
    }
    /// <summary>
    /// Adds the number of seconds.
    /// </summary>
    public BigDateTime AddSeconds(BigDecimal Value) {
        return new BigDateTime(TotalSeconds + Value);
    }
    /// <summary>
    /// Adds the number of milliseconds.
    /// </summary>
    public BigDateTime AddMilliseconds(BigDecimal Value) {
        return AddSeconds(Value / 1000);
    }
    /// <summary>
    /// Adds the number of microseconds.
    /// </summary>
    public BigDateTime AddMicroseconds(BigDecimal Value) {
        return AddMilliseconds(Value / 1000);
    }
    /// <summary>
    /// Adds the number of nanoseconds.
    /// </summary>
    public BigDateTime AddNanoseconds(BigDecimal Value) {
        return AddMicroseconds(Value / 1000);
    }
    /// <summary>
    /// Adds the total seconds of <paramref name="Value"/>.
    /// </summary>
    public BigDateTime Add(BigDateTime Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    /// <summary>
    /// Returns the number of seconds between this and <paramref name="Value"/>.
    /// </summary>
    public BigDecimal Subtract(BigDateTime Value) {
        return TotalSeconds - Value.TotalSeconds;
    }
    /// <summary>
    /// Returns the full name of the month according to the given or invariant culture.<br/>
    /// Example: January
    /// </summary>
    public string MonthName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.InvariantCulture;
        return Culture.DateTimeFormat.GetMonthName(Month);
    }
    /// <summary>
    /// Returns the abbreviated name of the month according to the given or invariant culture.<br/>
    /// Example: Jan
    /// </summary>
    public string AbbreviatedMonthName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.InvariantCulture;
        return Culture.DateTimeFormat.GetAbbreviatedMonthName(Month);
    }
    /// <summary>
    /// Returns the full name of the day of the week according to the given or invariant culture.<br/>
    /// Example: Monday
    /// </summary>
    public string DayOfWeekName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.InvariantCulture;
        return Culture.DateTimeFormat.GetDayName(DayOfWeek);
    }
    /// <summary>
    /// Returns the abbreviated name of the day of the week according to the given or invariant culture.<br/>
    /// Example: Mon
    /// </summary>
    public string AbbreviatedDayOfWeekName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.InvariantCulture;
        return Culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek);
    }
    /// <summary>
    /// Returns the name of the daytime segment according to the given or invariant culture.<br/>
    /// Example: AM
    /// </summary>
    public string DaytimeSegmentName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.InvariantCulture;
        return DaytimeSegment switch {
            DaytimeSegment.AM => Culture.DateTimeFormat.AMDesignator,
            DaytimeSegment.PM => Culture.DateTimeFormat.PMDesignator,
            _ => throw new Exception($"Invalid daytime segment ('{DaytimeSegment}')")
        };
    }
    /// <summary>
    /// Stringifies the date and time using a format string
    /// (see <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings">Format Strings</see>).
    /// </summary>
    public string ToString(string Format) {
        return new BigDateTimeOffset(this).ToString(Format);
    }
    /// <summary>
    /// Stringifies the date and time like "1970/01/01 00:00:00".
    /// </summary>
    public override string ToString() {
        return ToString("yyyy/MM/dd HH:mm:ss");
    }
    /// <summary>
    /// Stringifies the date and time like "Thursday 1 January 1970 00:00:00".
    /// </summary>
    public string ToLongString() {
        return ToString("dddd d MMMM yyyy HH:mm:ss");
    }
    /// <summary>
    /// Stringifies the date and time like "Thu 1 Jan 1970 00:00:00".
    /// </summary>
    public string ToShortString() {
        return ToString("ddd d MMM yyyy HH:mm:ss");
    }

    /// <summary>
    /// Compares this <see cref="BigDateTime"/> with the <see cref="BigDateTime"/> for sorting.
    /// </summary>
    public int CompareTo(BigDateTime Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds);
    }
    /// <summary>
    /// Compares this <see cref="BigDateTime"/> with the <see cref="DateTime"/> for sorting.
    /// </summary>
    public int CompareTo(DateTime Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds());
    }

    /// <summary>
    /// Calculates the year.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public BigInteger Year {
        get => BlackMagic.GetYear(TotalSeconds.WholeValue);
    }
    /// <summary>
    /// Calculates the month of the year.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public int Month {
        get => BlackMagic.GetMonthOfYear(TotalSeconds.WholeValue);
    }
    /// <summary>
    /// Calculates the day of the month.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public int Day {
        get => BlackMagic.GetDayOfMonth(TotalSeconds.WholeValue);
    }
    /// <summary>
    /// Calculates the hour.
    /// </summary>
    public int Hour {
        get => (int)(TotalSeconds.WholeValue % SecondsInDay / SecondsInHour);
    }
    /// <summary>
    /// Calculates the minute.
    /// </summary>
    public int Minute {
        get => (int)(TotalSeconds.WholeValue % SecondsInHour / SecondsInMinute);
    }
    /// <summary>
    /// Calculates the second.
    /// </summary>
    public BigDecimal Second {
        get => TotalSeconds % SecondsInMinute;
    }
    /// <summary>
    /// Calculates the day of the year.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public int DayOfYear {
        get => BlackMagic.GetDayOfYear(TotalSeconds.WholeValue);
    }
    /// <summary>
    /// Calculates the day of the week.
    /// </summary>
    public DayOfWeek DayOfWeek {
        get => (DayOfWeek)(int)(TotalSeconds.WholeValue % DaysInWeek / SecondsInDay);
    }
    /// <summary>
    /// Calculates the daytime segment (AM/PM).
    /// </summary>
    public DaytimeSegment DaytimeSegment {
        get => (DaytimeSegment)(Hour / HoursInDaytimeSegment);
    }

    /// <summary>
    /// Parses a <see cref="BigDateTime"/> from the string or throws an exception.
    /// </summary>
    /// <remarks>Currently only parses strings in the format "y/M/d H:m:s", where each component is optional and each separator is interchangeable.</remarks>
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
    /// <summary>
    /// Parses a <see cref="BigDateTime"/> from the string or returns false.
    /// </summary>
    /// <remarks>Currently only parses strings in the format "y/M/d H:m:s", where each component is optional and each separator is interchangeable.</remarks>
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
    /// <summary>
    /// Returns a <see cref="BigDateTime"/> representing the current time at the given UTC offset.
    /// </summary>
    public static BigDateTime Now(BigDecimal Offset) {
        return new BigDateTime(DateTime.UtcNow).AddHours(Offset);
    }
    /// <summary>
    /// Returns a <see cref="BigDateTime"/> representing the current time at the local UTC offset.
    /// </summary>
    public static BigDateTime Now() {
        return Now(DateTimeOffset.Now.Offset.TotalHours);
    }

    /// <inheritdoc cref="Add(BigDateTime)"/>
    public static BigDateTime operator +(BigDateTime This, BigDateTime Other) {
        return This.Add(Other);
    }
    /// <inheritdoc cref="Subtract(BigDateTime)"/>
    public static BigDecimal operator -(BigDateTime This, BigDateTime Other) {
        return This.Subtract(Other);
    }
    /// <summary>
    /// Converts the <see cref="DateTime"/> to a <see cref="BigDateTime"/>.
    /// </summary>
    public static implicit operator BigDateTime(DateTime DateTime) {
        return new BigDateTime(DateTime);
    }
    /// <summary>
    /// Converts the <see cref="BigDateTime"/> to a <see cref="DateTime"/>.
    /// </summary>
    /// <remarks>
    /// If <see cref="Second"/> is very precise, it loses precision.<br/>
    /// If <see cref="Year"/> is outside 1 to 9999, an exception is thrown.
    /// </remarks>
    public static explicit operator DateTime(BigDateTime BigDateTime) {
        return new DateTime(TimeSpan.FromSeconds((double)BigDateTime.TotalSeconds - SecondsInCommonYear).Ticks); // Subtract seconds in year 0, because DateTime starts at year 1
    }
}