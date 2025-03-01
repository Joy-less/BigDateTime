namespace ExtendedNumerics;

using static GregorianCalendarConstants;

internal static class Formatter {
    private static readonly Dictionary<string, Func<BigDateTimeOffset, object?>> FormatTable = new() {
        ["dddd"] = BDTO => BDTO.DayOfWeekName(),
        ["ddd"] = BDTO => BDTO.AbbreviatedDayOfWeekName(),
        ["dd"] = BDTO => BDTO.Day.ToString().PadLeft(2, '0'),
        ["d"] = BDTO => BDTO.Day,
        ["ffffffff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second),
        ["fffffff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(7, '0')[..6],
        ["ffffff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(6, '0')[..5],
        ["fffff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(5, '0')[..4],
        ["ffff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(4, '0')[..3],
        ["fff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(3, '0')[..2],
        ["ff"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(2, '0')[..1],
        ["f"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString()[..0],
        ["FFFFFFFF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().NullIfZero(),
        ["FFFFFFF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(7, '0')[..6].NullIfZero(),
        ["FFFFFF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(6, '0')[..5].NullIfZero(),
        ["FFFFF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(5, '0')[..4].NullIfZero(),
        ["FFFF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(4, '0')[..3].NullIfZero(),
        ["FFF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(3, '0')[..2].NullIfZero(),
        ["FF"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString().PadLeft(2, '0')[..1].NullIfZero(),
        ["F"] = BDTO => BigReal.GetFractionalPart(BDTO.Second).ToString()[..0].NullIfZero(),
        ["hh"] = BDTO => (BDTO.Hour % HoursInDaytimeSegment).ToString().PadLeft(2, '0'),
        ["h"] = BDTO => BDTO.Hour % HoursInDaytimeSegment,
        ["HH"] = BDTO => BDTO.Hour.ToString("D2"),
        ["H"] = BDTO => BDTO.Hour,
        ["mm"] = BDTO => BDTO.Minute.ToString("D2"),
        ["m"] = BDTO => BDTO.Minute,
        ["MMMM"] = BDTO => BDTO.MonthName(),
        ["MMM"] = BDTO => BDTO.AbbreviatedMonthName(),
        ["MM"] = BDTO => BDTO.Month.ToString("D2"),
        ["M"] = BDTO => BDTO.Month,
        ["ss"] = BDTO => BigReal.GetWholePart(BDTO.Second).ToString().PadLeft(2, '0'),
        ["s"] = BDTO => BigReal.GetWholePart(BDTO.Second),
        ["tt"] = BDTO => BDTO.DaytimeSegmentName(),
        ["t"] = BDTO => BDTO.DaytimeSegmentName().FirstOrDefault(),
        ["yyyyyy"] = BDTO => BDTO.Year,
        ["yyyyy"] = BDTO => BDTO.Year.ToString("D5"),
        ["yyyy"] = BDTO => BDTO.Year.ToString("D4"),
        ["yyy"] = BDTO => BDTO.Year.ToString("D3"),
        ["yy"] = BDTO => string.Concat(BDTO.Year.ToString().TakeLast(2)).PadLeft(2, '0'),
        ["y"] = BDTO => string.Concat(BDTO.Year.ToString().TakeLast(2)),
        ["zzz"] = BDTO => (BigReal.IsNegative(BDTO.Offset) ? "-" : "+") + TimeSpan.FromHours(Math.Abs((double)BDTO.Offset)).ToString("hh':'mm"),
        ["zz"] = BDTO => ((BigReal.IsNegative(BDTO.Offset) ? "-" : "+") + BigReal.GetWholePart(BigReal.Abs(BDTO.Offset))).PadLeft(2, '0'),
        ["z"] = BDTO => (BigReal.IsNegative(BDTO.Offset) ? "-" : "+") + BigReal.GetWholePart(BigReal.Abs(BDTO.Offset)),
    };

    public static string Format(string Format, BigDateTimeOffset BigDateTimeOffset) {
        for (int Index = 0; Index < Format.Length; Index++) {
            foreach ((string Find, Func<BigDateTimeOffset, object?> Replacement) in FormatTable) {
                // Ensure find sequence would fit inside rest of string
                if (Index + Find.Length > Format.Length) {
                    continue;
                }
                // Ensure find sequence follows current index
                if (Format[Index..(Index + Find.Length)] != Find) {
                    continue;
                }
                // Get replacement string
                string ReplacementStr = Replacement(BigDateTimeOffset)?.ToString() ?? "";
                // Replace find sequence
                Format = Format[..Index] + ReplacementStr + Format[(Index + Find.Length)..];
                Index += ReplacementStr.Length;
                // Replace again
                Index--;
                break;
            }
        }
        return Format;
    }

    private static string? NullIfZero(this string Number) {
        return BigReal.IsZero(BigReal.Parse(Number)) ? null : Number;
    }
}