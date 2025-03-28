﻿using System.Globalization;
using System.Numerics;

namespace ExtendedNumerics;

using static GregorianCalendarConstants;
using static Extensions;

/// <summary>
/// An arbitrary size and precision DateTime in the Gregorian calendar.
/// </summary>
public readonly struct BigDateTime(BigReal TotalSeconds) : IComparable, IComparable<BigDateTime>, IComparable<DateTime> {
    /// <summary>
    /// The total number of seconds since 0000/00/00 00:00:00.
    /// </summary>
    public BigReal TotalSeconds { get; } = TotalSeconds;

    /// <summary>
    /// Constructs a <see cref="BigDateTime"/> from a date and time.
    /// </summary>
    public BigDateTime(BigInteger Year, int Month, int Day, int Hour, int Minute, BigReal Second)
        : this(TotalSecondsAt(Year, Month, Day, Hour, Minute, Second)) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTime"/> from a date.
    /// </summary>
    public BigDateTime(BigInteger Year, int Month, int Day)
        : this(Year, Month, Day, 0, 0, BigReal.Zero) { }
    /// <summary>
    /// Constructs a <see cref="BigDateTime"/> from a less-flexible <see cref="DateTime"/>.
    /// </summary>
    public BigDateTime(DateTime DateTime)
        : this(DateTime.TotalSeconds()) { }

    /// <summary>
    /// Calculates the year.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public BigInteger Year {
        get => BlackMagic.GetYear(BigReal.GetWholePart(TotalSeconds));
    }
    /// <summary>
    /// Calculates the month of the year.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public int Month {
        get => BlackMagic.GetMonthOfYear(BigReal.GetWholePart(TotalSeconds));
    }
    /// <summary>
    /// Calculates the day of the month.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public int Day {
        get => BlackMagic.GetDayOfMonth(BigReal.GetWholePart(TotalSeconds));
    }
    /// <summary>
    /// Calculates the hour.
    /// </summary>
    public int Hour {
        get => (int)(BigReal.GetWholePart(TotalSeconds) % SecondsInDay / SecondsInHour);
    }
    /// <summary>
    /// Calculates the minute.
    /// </summary>
    public int Minute {
        get => (int)(BigReal.GetWholePart(TotalSeconds) % SecondsInHour / SecondsInMinute);
    }
    /// <summary>
    /// Calculates the second.
    /// </summary>
    public BigReal Second {
        get => TotalSeconds % SecondsInMinute;
    }
    /// <summary>
    /// Calculates the day of the year.
    /// </summary>
    /// <remarks>This operation uses black magic.</remarks>
    public int DayOfYear {
        get => BlackMagic.GetDayOfYear(BigReal.GetWholePart(TotalSeconds));
    }
    /// <summary>
    /// Calculates the day of the week.
    /// </summary>
    public DayOfWeek DayOfWeek {
        get => (DayOfWeek)(((uint)(BigReal.GetWholePart(TotalSeconds) / SecondsInDay)) % DaysInWeek);
    }
    /// <summary>
    /// Calculates the daytime segment (AM/PM).
    /// </summary>
    public DaytimeSegment DaytimeSegment {
        get => (DaytimeSegment)(Hour / HoursInDaytimeSegment);
    }
    /// <summary>
    /// Calculates the date time at midnight.
    /// </summary>
    public BigDateTime Date {
        get => new(TotalSeconds - (TotalSeconds % SecondsInDay));
    }

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
    public BigDateTime AddDays(BigReal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInDay));
    }
    /// <summary>
    /// Adds the number of hours.
    /// </summary>
    public BigDateTime AddHours(BigReal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInHour));
    }
    /// <summary>
    /// Adds the number of minutes.
    /// </summary>
    public BigDateTime AddMinutes(BigReal Value) {
        return new BigDateTime(TotalSeconds + (Value * SecondsInMinute));
    }
    /// <summary>
    /// Adds the number of seconds.
    /// </summary>
    public BigDateTime AddSeconds(BigReal Value) {
        return new BigDateTime(TotalSeconds + Value);
    }
    /// <summary>
    /// Adds the number of milliseconds.
    /// </summary>
    public BigDateTime AddMilliseconds(BigReal Value) {
        return AddSeconds(Value / 1000);
    }
    /// <summary>
    /// Adds the number of microseconds.
    /// </summary>
    public BigDateTime AddMicroseconds(BigReal Value) {
        return AddMilliseconds(Value / 1000);
    }
    /// <summary>
    /// Adds the number of nanoseconds.
    /// </summary>
    public BigDateTime AddNanoseconds(BigReal Value) {
        return AddMicroseconds(Value / 1000);
    }
    /// <summary>
    /// Adds the number of picoseconds.
    /// </summary>
    public BigDateTime AddPicoseconds(BigReal Value) {
        return AddNanoseconds(Value / 1000);
    }
    /// <summary>
    /// Adds the total seconds of <paramref name="Value"/>.
    /// </summary>
    public BigDateTime Add(BigDateTime Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    /// <summary>
    /// Adds the total seconds of <paramref name="Value"/>.
    /// </summary>
    public BigDateTime Add(TimeSpan Value) {
        return AddSeconds(Value.TotalSeconds);
    }
    /// <summary>
    /// Returns the number of seconds between this and <paramref name="Value"/>.
    /// </summary>
    public BigReal Subtract(BigDateTime Value) {
        return TotalSeconds - Value.TotalSeconds;
    }
    /// <summary>
    /// Returns a date time that subtracts the total seconds of <paramref name="Value"/>.
    /// </summary>
    public BigDateTime Subtract(TimeSpan Value) {
        return AddSeconds(-Value.TotalSeconds);
    }
    /// <summary>
    /// Returns the full name of the month according to the given or current culture.<br/>
    /// Example: January
    /// </summary>
    public string MonthName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.CurrentCulture;
        return Culture.DateTimeFormat.GetMonthName(Month);
    }
    /// <summary>
    /// Returns the abbreviated name of the month according to the given or current culture.<br/>
    /// Example: Jan
    /// </summary>
    public string AbbreviatedMonthName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.CurrentCulture;
        return Culture.DateTimeFormat.GetAbbreviatedMonthName(Month);
    }
    /// <summary>
    /// Returns the full name of the day of the week according to the given or current culture.<br/>
    /// Example: Monday
    /// </summary>
    public string DayOfWeekName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.CurrentCulture;
        return Culture.DateTimeFormat.GetDayName(DayOfWeek);
    }
    /// <summary>
    /// Returns the abbreviated name of the day of the week according to the given or current culture.<br/>
    /// Example: Mon
    /// </summary>
    public string AbbreviatedDayOfWeekName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.CurrentCulture;
        return Culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek);
    }
    /// <summary>
    /// Returns the name of the daytime segment according to the given or current culture.<br/>
    /// Example: AM
    /// </summary>
    public string DaytimeSegmentName(CultureInfo? Culture = null) {
        Culture ??= CultureInfo.CurrentCulture;
        return DaytimeSegment switch {
            DaytimeSegment.AM => Culture.DateTimeFormat.AMDesignator,
            DaytimeSegment.PM => Culture.DateTimeFormat.PMDesignator,
            _ => throw new ArgumentException($"Invalid daytime segment ('{DaytimeSegment}')")
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
    /// Returns:
    /// <list type="bullet">
    ///   <item>1 if this value &gt; <paramref name="Other"/></item>
    ///   <item>0 if this value == <paramref name="Other"/></item>
    ///   <item>-1 if this value &lt; <paramref name="Other"/></item>
    /// </list>
    /// </summary>
    public int CompareTo(BigDateTime Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds);
    }
    /// <inheritdoc cref="CompareTo(BigDateTime)"/>
    public int CompareTo(DateTime Other) {
        return TotalSeconds.CompareTo(Other.TotalSeconds());
    }
    /// <summary>
    /// Returns:
    /// <list type="bullet">
    ///   <item>1 if this value &gt; <paramref name="Other"/></item>
    ///   <item>0 if this value == <paramref name="Other"/></item>
    ///   <item>-1 if this value &lt; <paramref name="Other"/></item>
    ///   <item>1 if <paramref name="Other"/> is <see langword="null"/> (<see langword="null"/> is less than any value)</item>
    ///   <item>throws an exception if <paramref name="Other"/> is not <see cref="BigDateTime"/></item>
    /// </list>
    /// </summary>
    public int CompareTo(object? Other) {
        if (Other is null) {
            return 1;
        }
        else if (Other is BigDateTime OtherBigDateTime) {
            return CompareTo(OtherBigDateTime);
        }
        else {
            throw new ArgumentException($"{nameof(Other)} is not {nameof(BigDateTime)}");
        }
    }

    /// <summary>
    /// Parses a <see cref="BigDateTime"/> from the string or throws an exception.
    /// </summary>
    /// <remarks>Currently only parses strings in the format "y/M/d H:m:s", where each component is optional and each separator is interchangeable.</remarks>
    public static BigDateTime Parse(ReadOnlySpan<char> String) {
        Parser Parser = new(String);

        Parser.EatBigInteger(out BigInteger Year, 0);
        Parser.EatInt32(out int Month, 1);
        Parser.EatInt32(out int Day, 1);
        Parser.EatInt32(out int Hour, 0);
        Parser.EatInt32(out int Minute, 0);
        Parser.EatBigReal(out BigReal Second, 0);

        return new BigDateTime(Year, Month, Day, Hour, Minute, Second);
    }
    /// <summary>
    /// Parses a <see cref="BigDateTime"/> from the string or returns false.
    /// </summary>
    /// <remarks>Currently only parses strings in the format "y/M/d H:m:s", where each component is optional and each separator is interchangeable.</remarks>
    public static bool TryParse(ReadOnlySpan<char> String, out BigDateTime Result) {
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
    public static BigDateTime Now(BigReal Offset) {
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
    public static BigReal operator -(BigDateTime This, BigDateTime Other) {
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
    /// If <see cref="Second"/> is very precise, the precision is reduced.<br/>
    /// If <see cref="Year"/> is outside 1 to 9999, an <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </remarks>
    public static explicit operator DateTime(BigDateTime BigDateTime) {
        return new DateTime(TimeSpan.FromSeconds((double)BigDateTime.TotalSeconds - SecondsInCommonYear).Ticks); // Subtract seconds in year 0, because DateTime starts at year 1
    }
}