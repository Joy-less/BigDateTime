using System.Numerics;

namespace BigTime;

using static EarthConstants;

/*
    So you might be thinking: 「 What the hell is going on here? 」
    And you're not alone. I just took this stuff from DateTime and adapted it.
    Blame Pope Gregory XIII for introducing the Gregorian calendar.
*/
internal static class BlackMagic {
    // Euclidean Affine Functions Algorithm (EAF) constants
    private const uint EafMultiplier = (uint)(((1UL << 32) + DaysIn4Years - 1) / DaysIn4Years);
    private const uint EafDivider = EafMultiplier * 4;
    private const int DaysInYearFromMarch1st = 306;

    public static BigInteger Year(BigInteger TotalSeconds) {
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
    public static int MonthOfYear(BigInteger TotalSeconds) {
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
    public static int DayOfMonth(BigInteger TotalSeconds) {
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
    public static int DayOfYear(BigInteger TotalSeconds) {
        // Offset a year since the calculation starts at 1
        if (TotalSeconds >= SecondsInCommonYear) {
            TotalSeconds -= SecondsInCommonYear;
        }

        // Black magic
        return 1 + (int)((((uint)(((TotalSeconds / (SecondsInHour * 6)) | 3) % DaysIn400Years)) | 3) * EafMultiplier / EafDivider);
    }
}