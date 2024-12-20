﻿using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using ExtendedNumerics;

namespace BigTime;

internal struct Parser(string FromString) {
    public string FromString = FromString;
    public int Index = 0;

    private static readonly char[] Separators = [' ', '/', ':', '-', '　', '／', '：', 'ー'];

    public bool EatComponent([NotNullWhen(true)] out string? Component) {
        // Ensure there are more components
        if (Index >= FromString.Length) {
            Component = null;
            return false;
        }

        // Build component until separator reached
        StringBuilder Builder = new();
        while (Index < FromString.Length) {
            char Char = FromString[Index];

            if (Separators.Contains(Char)) {
                if (Builder.Length == 0) {
                    throw new Exception($"Expected digit before separator ('{Char}')");
                }
                Index++;
                break;
            }
            else {
                Builder.Append(Char);
                Index++;
            }
        }
        Component = Builder.ToString();
        return true;
    }
    public bool EatBigInteger(out BigInteger BigInteger, BigInteger Default = default) {
        if (EatComponent(out string? Component)) {
            BigInteger = BigInteger.Parse(Component);
            return true;
        }
        BigInteger = Default;
        return false;
    }
    public bool EatBigDecimal(out BigDecimal BigDecimal, BigDecimal Default = default) {
        if (EatComponent(out string? Component)) {
            BigDecimal = BigDecimal.Parse(Component);
            return true;
        }
        BigDecimal = Default;
        return false;
    }
    public bool EatInt(out int Int, int Default = default) {
        if (EatComponent(out string? Component)) {
            Int = int.Parse(Component);
            return true;
        }
        Int = Default;
        return false;
    }
}