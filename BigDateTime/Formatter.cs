using ExtendedNumerics;

namespace BigTime;

using static GregorianCalendarConstants;

internal static class Formatter {
    private static readonly Dictionary<string, Func<BigDateTimeOffset, object?>> FormatTable = new() {
        ["dddd"] = BDTO => BDTO.DayOfWeekName(),
        ["ddd"] = BDTO => BDTO.AbbreviatedDayOfWeekName(),
        ["dd"] = BDTO => BDTO.Day.ToString().PadLeft(2, '0'),
        ["d"] = BDTO => BDTO.Day,
        ["ffffffff"] = BDTO => BDTO.Second.GetFractionalPart(),
        ["fffffff"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(7, '0')[..6],
        ["ffffff"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(6, '0')[..5],
        ["fffff"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(5, '0')[..4],
        ["ffff"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(4, '0')[..3],
        ["fff"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(3, '0')[..2],
        ["ff"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(2, '0')[..1],
        ["f"] = BDTO => BDTO.Second.GetFractionalPart().ToString()[..0],
        ["FFFFFFFF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().NullIfZero(),
        ["FFFFFFF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(7, '0')[..6].NullIfZero(),
        ["FFFFFF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(6, '0')[..5].NullIfZero(),
        ["FFFFF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(5, '0')[..4].NullIfZero(),
        ["FFFF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(4, '0')[..3].NullIfZero(),
        ["FFF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(3, '0')[..2].NullIfZero(),
        ["FF"] = BDTO => BDTO.Second.GetFractionalPart().ToString().PadLeft(2, '0')[..1].NullIfZero(),
        ["F"] = BDTO => BDTO.Second.GetFractionalPart().ToString()[..0].NullIfZero(),
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
        ["ss"] = BDTO => BDTO.Second.WholeValue.ToString().PadLeft(2, '0'),
        ["s"] = BDTO => BDTO.Second.WholeValue,
        ["tt"] = BDTO => BDTO.DaytimeSegmentName(),
        ["t"] = BDTO => BDTO.DaytimeSegmentName().FirstOrDefault(),
        ["yyyyyy"] = BDTO => BDTO.Year,
        ["yyyyy"] = BDTO => BDTO.Year.ToString("D5"),
        ["yyyy"] = BDTO => BDTO.Year.ToString("D4"),
        ["yyy"] = BDTO => BDTO.Year.ToString("D3"),
        ["yy"] = BDTO => string.Concat(BDTO.Year.ToString().TakeLast(2)).PadLeft(2, '0'),
        ["y"] = BDTO => string.Concat(BDTO.Year.ToString().TakeLast(2)),
        ["zzz"] = BDTO => (BDTO.Offset.IsNegative() ? "-" : "+") + TimeSpan.FromHours((double)BigDecimal.Abs(BDTO.Offset)).ToString("hh':'mm"),
        ["zz"] = BDTO => ((BDTO.Offset.IsNegative() ? "-" : "+") + BigDecimal.Abs(BDTO.Offset).WholeValue).PadLeft(2, '0'),
        ["z"] = BDTO => (BDTO.Offset.IsNegative() ? "-" : "+") + BigDecimal.Abs(BDTO.Offset).WholeValue,
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
        return BigDecimal.Parse(Number).IsZero() ? null : Number;
    }
}