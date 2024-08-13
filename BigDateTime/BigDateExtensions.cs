using System.Collections;

namespace ExtendedNumerics;

internal static class BigDateTimeExtensions {
    /// <summary>
    /// Takes a format string (e.g. "yyyy"), finds a character (e.g. 'y') and replaces it with a value (e.g. 2024).
    /// </summary>
    public static string Format(this string Format, char Find, object Replace) {
        for (int Index = 0; Index < Format.Length; Index++) {
            // Ensure placeholder found
            if (Format[Index] != Find) {
                continue;
            }
            // Find end of placeholder
            int StartIndex = Index;
            while (Index < Format.Length && Format[Index] == Find) {
                Index++;
            }
            int PlaceholderLength = Index - StartIndex;
            // Convert replacement to string
            string ReplaceStr = Replace.ToString() ?? "";

            // Calculate minimum replacement length (adding decimal places)
            int MinReplaceLength = PlaceholderLength;
            int DecimalPlaceIndex = ReplaceStr.IndexOf('.');
            if (DecimalPlaceIndex >= 0) {
                MinReplaceLength += ReplaceStr.Length - DecimalPlaceIndex;
            }
            // Pad replacement with zeroes to fill placeholder
            ReplaceStr = ReplaceStr.PadLeft(MinReplaceLength, '0');

            // Replace placeholder
            Format = Format[..StartIndex] + ReplaceStr + Format[Index..];
        }
        return Format;
    }
    /// <summary>
    /// Takes a date string (e.g. "2024/08/12 20:24:03") and separates it into up to <see cref="Count"/> components.
    /// </summary>
    public static List<BigDecimal> ParseDateComponents(this string Date, int Count, int[]? IntegerComponents = null, char[]? Separators = null) {
        IntegerComponents ??= [0, 1, 2];
        Separators ??= ['-', '/', ':'];

        // Split date into components
        List<BigDecimal> Components = new(Count);
        Components.AddRange(Date.Split(Separators).Select(BigDecimal.Parse));

        // Ensure integer components are whole
        foreach (int Index in IntegerComponents) {
            if (Components[Index].DecimalPlaces != 0) {
                throw new Exception($"Component {Index} must be an integer.");
            }
        }

        // Assume missing components are zero
        for (int Counter = Components.Count; Counter < Count; Counter++) {
            Components.Add(0);
        }
        return Components;
    }
}