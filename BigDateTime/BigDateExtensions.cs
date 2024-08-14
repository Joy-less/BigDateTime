using System.Numerics;

namespace ExtendedNumerics;

internal static class BigDateTimeExtensions {
    /// <summary>
    /// Returns null if the string is zero, otherwise the string.
    /// </summary>
    public static string? NullIfZero(this string Number) {
        return BigDecimal.Parse(Number).IsZero() ? null : Number;
    }
    /// <summary>
    /// Gets the value at the array index as a string and parses it as a BigInteger, otherwise returns 0.
    /// </summary>
    public static BigInteger ParseBigIntegerOrDefault(this Array Array, int Index) {
        if (Index < 0 || Index >= Array.Length) {
            return default;
        }
        if (Array.GetValue(Index) is not string Value) {
            return default;
        }
        return BigInteger.Parse(Value);
    }
    /// <summary>
    /// Gets the value at the array index as a string and parses it as a BigDecimal, otherwise returns 0.
    /// </summary>
    public static BigDecimal ParseBigDecimalOrDefault(this Array Array, int Index) {
        if (Index < 0 || Index >= Array.Length) {
            return default;
        }
        if (Array.GetValue(Index) is not string Value) {
            return default;
        }
        return BigDecimal.Parse(Value);
    }
}