using System.Buffers;
using System.Numerics;

namespace ExtendedNumerics;

internal ref struct Parser(ReadOnlySpan<char> CharSpan) {
    public ReadOnlySpan<char> CharSpan { get; } = CharSpan;
    public int Index { get; set; } = 0;

    private static readonly SearchValues<char> Separators = SearchValues.Create([' ', '/', ':', '-', '　', '／', '：', 'ー']);

    public bool EatComponent(out ReadOnlySpan<char> Component) {
        // Find separator
        int SeparatorIndex = CharSpan[Index..].IndexOfAny(Separators);
        // Offset separator index from index
        if (SeparatorIndex >= 0) {
            SeparatorIndex += Index;
        }

        // No separator found
        if (SeparatorIndex < 0) {
            // Get last component
            Component = CharSpan[Index..];
            // Move past last component
            Index = CharSpan.Length;
        }
        // Separator found
        else {
            // Get component before separator
            Component = CharSpan[Index..SeparatorIndex];
            // Move past component
            Index = SeparatorIndex + 1;
        }

        // Return whether component was consumed
        return !Component.IsEmpty;
    }
    public bool EatBigInteger(out BigInteger BigInteger, BigInteger Default = default) {
        if (EatComponent(out ReadOnlySpan<char> Component)) {
            BigInteger = BigInteger.Parse(Component);
            return true;
        }
        BigInteger = Default;
        return false;
    }
    public bool EatBigDecimal(out BigDecimal BigDecimal, BigDecimal Default = default) {
        if (EatComponent(out ReadOnlySpan<char> Component)) {
            BigDecimal = BigDecimal.Parse(Component.ToString()); // TODO: Use ReadOnlySpan<char> overload rather than calling ToString() if/when it is added.
            return true;
        }
        BigDecimal = Default;
        return false;
    }
    public bool EatInt32(out int Int, int Default = default) {
        if (EatComponent(out ReadOnlySpan<char> Component)) {
            Int = int.Parse(Component);
            return true;
        }
        Int = Default;
        return false;
    }
}