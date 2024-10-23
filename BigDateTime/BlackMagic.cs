using System.Numerics;

namespace BigTime;

using static EarthConstants;

/*
    So you might be thinking: 「 What the hell am I looking at? 」
    You're not alone. I just took stuff from DateTime and adapted it.
    Blame Pope Gregory XIII for introducing the Gregorian calendar.
*/
public static class BlackMagic {
    // Euclidean Affine Functions Algorithm (EAF) constants
    private const uint EafMultiplier = (uint)(((1UL << 32) + DaysIn4Years - 1) / DaysIn4Years);
    private const uint EafDivider = EafMultiplier * 4;
    private const int DaysInYearFromMarch1st = 306;

    public static BigInteger GetYear(BigInteger TotalSeconds) {
        BigInteger Century = BigInteger.DivRem(
            (TotalSeconds / (SecondsInHour * 6)) | 3,
            DaysIn400Years,
            out BigInteger DayInCentury
        );
        return (100 * Century) + ((DayInCentury | 3) / DaysIn4Years);
    }
    public static int GetMonthOfYear(BigInteger TotalSeconds) {
        int R1 = (int)((((TotalSeconds / (SecondsInHour * 6)) | 3) + 1224) % DaysIn400Years);
        ulong U2 = (ulong)Math.BigMul((int)EafMultiplier, R1 | 3);
        ushort DaysSinceMarch1st = (ushort)((uint)U2 / EafDivider);
        int N3 = (2141 * DaysSinceMarch1st) + 197913;
        return (ushort)(N3 >> 16) - (DaysSinceMarch1st >= DaysInYearFromMarch1st ? 12 : 0);
    }
    public static int GetDayOfMonth(BigInteger TotalSeconds) {
        int R1 = (int)((((TotalSeconds / (SecondsInHour * 6)) | 3) + 1224) % DaysIn400Years);
        ulong U2 = (ulong)Math.BigMul((int)EafMultiplier, R1 | 3);
        ushort DaysSinceMarch1st = (ushort)((uint)U2 / EafDivider);
        int N3 = (2141 * DaysSinceMarch1st) + 197913;
        return ((ushort)N3 / 2141) + 1;
    }
    public static int GetDayOfYear(BigInteger TotalSeconds) {
        return 1 + (int)((((uint)(((TotalSeconds / (SecondsInHour * 6)) | 3) % DaysIn400Years)) | 3) * EafMultiplier / EafDivider);
    }
    public static (BigInteger Year, int Month, int Day) GetDate(BigInteger TotalSeconds) {
        BigInteger Century = BigInteger.DivRem(((TotalSeconds / (SecondsInHour * 6)) | 3) + 1224, DaysIn400Years, out BigInteger R1);
        ulong U2 = (ulong)(EafMultiplier * (R1 | 3));
        ushort DaysSinceMarch1st = (ushort)((uint)U2 / EafDivider);
        int N3 = 2141 * DaysSinceMarch1st + 197913;
        BigInteger Year = 100 * Century + (uint)(U2 >> 32);
        // Compute month and day
        int Month = (ushort)(N3 >> 16);
        int Day = (ushort)N3 / 2141 + 1;
        // Rollover December 31
        if (DaysSinceMarch1st >= DaysInYearFromMarch1st) {
            Year++;
            Month -= 12;
        }
        return (Year, Month, Day);
    }
}