using System.Collections.Immutable;
using System.Globalization;

namespace ExtendedNumerics;

using static GregorianCalendarConstants;

internal static class Formatter {
    private static readonly ImmutableArray<(string Find, Func<BigDateTimeOffset, string> Replace)> FormatTable = [
        ("dddd", BDTO => BDTO.DayOfWeekName()),
        ("ddd", BDTO => BDTO.AbbreviatedDayOfWeekName()),
        ("dd", BDTO => BDTO.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')),
        ("d", BDTO => BDTO.Day.ToString(CultureInfo.InvariantCulture)),
        ("fffffff", BDTO => FractionDigits(BDTO, 7)),
        ("ffffff", BDTO => FractionDigits(BDTO, 6)),
        ("fffff", BDTO => FractionDigits(BDTO, 5)),
        ("ffff", BDTO => FractionDigits(BDTO, 4)),
        ("fff", BDTO => FractionDigits(BDTO, 3)),
        ("ff", BDTO => FractionDigits(BDTO, 2)),
        ("f", BDTO => FractionDigits(BDTO, 1)),
        ("FFFFFFF", BDTO => FractionDigits(BDTO, 7).TrimEnd('0')),
        ("FFFFFF", BDTO => FractionDigits(BDTO, 6).TrimEnd('0')),
        ("FFFFF", BDTO => FractionDigits(BDTO, 5).TrimEnd('0')),
        ("FFFF", BDTO => FractionDigits(BDTO, 4).TrimEnd('0')),
        ("FFF", BDTO => FractionDigits(BDTO, 3).TrimEnd('0')),
        ("FF", BDTO => FractionDigits(BDTO, 2).TrimEnd('0')),
        ("F", BDTO => FractionDigits(BDTO, 1).TrimEnd('0')),
        ("hh", BDTO => (BDTO.Hour % HoursInDaytimeSegment).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')),
        ("h", BDTO => (BDTO.Hour % HoursInDaytimeSegment).ToString(CultureInfo.InvariantCulture)),
        ("HH", BDTO => BDTO.Hour.ToString("D2", CultureInfo.InvariantCulture)),
        ("H", BDTO => BDTO.Hour.ToString(CultureInfo.InvariantCulture)),
        ("mm", BDTO => BDTO.Minute.ToString("D2", CultureInfo.InvariantCulture)),
        ("m", BDTO => BDTO.Minute.ToString(CultureInfo.InvariantCulture)),
        ("MMMM", BDTO => BDTO.MonthName()),
        ("MMM", BDTO => BDTO.AbbreviatedMonthName()),
        ("MM", BDTO => BDTO.Month.ToString("D2", CultureInfo.InvariantCulture)),
        ("M", BDTO => BDTO.Month.ToString(CultureInfo.InvariantCulture)),
        ("ss", BDTO => BigReal.GetWholePart(BDTO.Second).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')),
        ("s", BDTO => BigReal.GetWholePart(BDTO.Second).ToString(CultureInfo.InvariantCulture)),
        ("tt", BDTO => BDTO.DaytimeSegmentName()),
        ("t", BDTO => BDTO.DaytimeSegmentName() is { Length: > 0 } DaytimeSegmentNameChar ? DaytimeSegmentNameChar[..1] : ""),
        ("yyyyyy", BDTO => BDTO.Year.ToString(CultureInfo.InvariantCulture)),
        ("yyyyy", BDTO => BDTO.Year.ToString("D5", CultureInfo.InvariantCulture)),
        ("yyyy", BDTO => BDTO.Year.ToString("D4", CultureInfo.InvariantCulture)),
        ("yyy", BDTO => BDTO.Year.ToString("D3", CultureInfo.InvariantCulture)),
        ("yy", BDTO => string.Concat(BDTO.Year.ToString(CultureInfo.InvariantCulture).TakeLast(2)).PadLeft(2, '0')),
        ("y", BDTO => string.Concat(BDTO.Year.ToString(CultureInfo.InvariantCulture).TakeLast(2))),
        ("zzz", BDTO => (BigReal.IsNegative(BDTO.Offset) ? "-" : "+") + TimeSpan.FromHours(Math.Abs((double)BDTO.Offset)).ToString("hh':'mm", CultureInfo.InvariantCulture)),
        ("zz", BDTO => ((BigReal.IsNegative(BDTO.Offset) ? "-" : "+") + BigReal.GetWholePart(BigReal.Abs(BDTO.Offset))).PadLeft(2, '0')),
        ("z", BDTO => (BigReal.IsNegative(BDTO.Offset) ? "-" : "+") + BigReal.GetWholePart(BigReal.Abs(BDTO.Offset))),
    ];

    public static string Format(string Format, BigDateTimeOffset BigDateTimeOffset) {
        for (int Index = 0; Index < Format.Length; Index++) {
            foreach ((string Find, Func<BigDateTimeOffset, string> Replace) in FormatTable) {
                // Ensure find sequence would fit inside rest of string
                if (Index + Find.Length > Format.Length) {
                    continue;
                }
                // Ensure find sequence follows current index
                if (Format[Index..(Index + Find.Length)] != Find) {
                    continue;
                }
                // Get replacement string
                string ReplacementStr = Replace(BigDateTimeOffset);
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

    private static string FractionDigits(BigDateTimeOffset Value, int Length) {
        string Digits = TrimPrefix(BigReal.GetFractionalPart(Value.Second).ToString(15, CultureInfo.InvariantCulture, padDecimal: true), "0.");
        return Digits.PadRight(Length, '0')[..Length];
    }
    private static string TrimPrefix(this string String, string Prefix) {
        if (String.StartsWith(Prefix, StringComparison.Ordinal)) {
            return String[Prefix.Length..];
        }
        return String;
    }
}