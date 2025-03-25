using System.Numerics;

namespace ExtendedNumerics;

using static GregorianCalendarConstants;

/// <summary>
/// So you might be thinking: 「 What the hell is going on here? 」<br/>
/// And you're not alone. I just took this stuff from <see cref="DateTime"/> and adapted it.<br/>
/// Blame Pope Gregory XIII for introducing the Gregorian calendar.
/// </summary>
internal static class BlackMagic {
    // Euclidean Affine Functions Algorithm (EAF) constants
    private const uint EafMultiplier = (uint)(((1UL << 32) + DaysIn4Years - 1) / DaysIn4Years);
    private const uint EafDivider = EafMultiplier * 4;
    private const int DaysInYearFromMarch1st = 306;

    /// <summary>
    /// Calculates the year number from the total seconds.<br/>
    /// <br/>
    /// Taken from <see cref="DateTime"/>:
    /// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTime.cs#L1564">Source Reference</see>
    /// </summary>
    public static BigInteger GetYear(BigInteger TotalSeconds) {
        // Offset a year since the calculation starts at 1
        if (TotalSeconds < SecondsInCommonYear) {
            TotalSeconds -= SecondsInCommonYear;
        }

        // Black magic
        BigInteger Century = BigInteger.DivRem(
            (TotalSeconds / (SecondsInHour * 6)) | 3,
            DaysIn400Years,
            out BigInteger DayInCentury
        );
        return (100 * Century) + ((DayInCentury | 3) / DaysIn4Years);
    }
    /// <summary>
    /// Calculates the month number in the year from the total seconds.<br/>
    /// <br/>
    /// Taken from <see cref="DateTime"/>:
    /// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTime.cs#L1505">Source Reference</see>
    /// </summary>
    public static int GetMonthOfYear(BigInteger TotalSeconds) {
        // Offset a year since the calculation starts at 1
        if (TotalSeconds >= SecondsInCommonYear) {
            TotalSeconds -= SecondsInCommonYear;
        }

        // Black magic
        int R1 = (int)((((TotalSeconds / (SecondsInHour * 6)) | 3) + 1224) % DaysIn400Years);
        ulong U2 = (ulong)Math.BigMul((int)EafMultiplier, R1 | 3);
        ushort DaysSinceMarch1st = (ushort)((uint)U2 / EafDivider);
        int N3 = (2141 * DaysSinceMarch1st) + 197913;
        return (ushort)(N3 >> 16) - (DaysSinceMarch1st >= DaysInYearFromMarch1st ? 12 : 0);
    }
    /// <summary>
    /// Calculates the day number in the month from the total seconds.<br/>
    /// <br/>
    /// Taken from <see cref="DateTime"/>:
    /// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTime.cs#L1428">Source Reference</see>
    /// </summary>
    public static int GetDayOfMonth(BigInteger TotalSeconds) {
        // Offset a year since the calculation starts at 1
        if (TotalSeconds >= SecondsInCommonYear) {
            TotalSeconds -= SecondsInCommonYear;
        }

        // Black magic
        int R1 = (int)((((TotalSeconds / (SecondsInHour * 6)) | 3) + 1224) % DaysIn400Years);
        ulong U2 = (ulong)Math.BigMul((int)EafMultiplier, R1 | 3);
        ushort DaysSinceMarch1st = (ushort)((uint)U2 / EafDivider);
        int N3 = (2141 * DaysSinceMarch1st) + 197913;
        return ((ushort)N3 / 2141) + 1;
    }
    /// <summary>
    /// Calculates the day number in the year from the total seconds.<br/>
    /// <br/>
    /// Taken from <see cref="DateTime"/>:
    /// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTime.cs#L1452">Source Reference</see>
    /// </summary>
    public static int GetDayOfYear(BigInteger TotalSeconds) {
        // Offset a year since the calculation starts at 1
        if (TotalSeconds >= SecondsInCommonYear) {
            TotalSeconds -= SecondsInCommonYear;
        }

        // Black magic
        return 1 + (int)((((uint)(((TotalSeconds / (SecondsInHour * 6)) | 3) % DaysIn400Years)) | 3) * EafMultiplier / EafDivider);
    }
    /// <summary>
    /// Calculates the number of days to the year since 0000/00/00 00:00:00.<br/>
    /// <br/>
    /// Taken from <see cref="DateTime"/>:
    /// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTime.cs#L1103">Source Reference</see>
    /// </summary>
    public static BigInteger GetDaysBeforeYear(BigInteger Year) {
        // Black magic
        BigInteger Century = Year / 100;
        return Year * (365 * 4 + 1) / 4 - Century + Century / 4;
    }
    /// <summary>
    /// Calculates the total seconds with the months added.<br/>
    /// <br/>
    /// Taken from <see cref="DateTime"/> (in .NET Framework 4.8 because the .NET 8 version is hardcoded to use ticks):
    /// <see href="https://referencesource.microsoft.com/#mscorlib/system/datetime.cs,02ff6ea643eb71e8,references">Source Reference</see>
    /// </summary>
    public static BigReal AddMonths(BigReal TotalSeconds, BigInteger Months) {
        GetDate(BigReal.GetWholePart(TotalSeconds), out BigInteger Year, out int Month, out int Day);

        // Black magic
        BigInteger I = Month - 1 + Months;
        if (I >= 0) {
            Month = (int)(I % 12) + 1;
            Year += I / 12;
        }
        else {
            Month = 12 + (int)((I + 1) % 12);
            Year += (I - 11) / 12;
        }
        int Days = DaysInMonth(Year, Month);
        if (Day > Days) {
            Day = Days;
        }

        // Return date total seconds + time total seconds
        return new BigDateTime(Year, Month, Day).TotalSeconds
            + (TotalSeconds % SecondsInDay);
    }

    /// <summary>
    /// Calculates each component and returns them as out parameters for conciseness.
    /// </summary>
    private static void GetDate(BigInteger TotalSeconds, out BigInteger Year, out int Month, out int Day) {
        Year = GetYear(TotalSeconds);
        Month = GetMonthOfYear(TotalSeconds);
        Day = GetDayOfMonth(TotalSeconds);
    }
}