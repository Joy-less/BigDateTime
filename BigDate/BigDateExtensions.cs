using ExtendedNumerics;

namespace BigDate;

internal static class BigDateExtensions {
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
    public static List<BigDecimal> ParseDateComponents(this string Date, int Count, int[]? IntegerComponents = null, char[]? Separators = null) {
        IntegerComponents ??= [0, 1, 2];
        Separators ??= ['-', '/', ':'];

        // Split date into components
        string[] Components = Date.Split(Separators);

        // Yield date components
        List<BigDecimal> DecimalComponents = new(Count);
        for (int Index = 0; Index < Components.Length; Index++) {
            // Parse component as decimal
            BigDecimal DecimalComponent = BigDecimal.Parse(Components[Index]);
            // Ensure component is integer if required
            if (IntegerComponents.Contains(Index)) {
                if (!DecimalComponent.GetFractionalPart().IsZero()) {
                    throw new Exception($"Component {Index} must be an integer.");
                }
            }
            // Yield date component
            DecimalComponents.Add(DecimalComponent);
        }

        // Assume zero for missing components
        for (int Index = DecimalComponents.Count; Index < Count; Index++) {
            DecimalComponents.Add(BigDecimal.Zero);
        }
        return DecimalComponents;
    }
}