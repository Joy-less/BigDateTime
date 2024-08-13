using System.Numerics;
using ExtendedNumerics;

namespace BigDate;

[Serializable]
public readonly struct BigDateTimeOffset : IComparable, IComparable<BigDateTimeOffset> {
    public readonly BigDateTime BigDateTime;
    public readonly BigDecimal Offset;

    public BigDateTimeOffset(BigDateTime BigDateTime, BigDecimal? Offset = null) {
        this.BigDateTime = BigDateTime;
        this.Offset = Offset ?? BigDecimal.Zero;
    }
    public BigDateTimeOffset(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal Hour, BigDecimal Minute, BigDecimal Second, BigDecimal? Offset = null, BigDatePlanet? Planet = null)
        : this(new BigDateTime(Year, Month, Day, Hour, Minute, Second, Planet), Offset)
    {
    }
    public BigDateTimeOffset(BigInteger Year, BigInteger Month, BigInteger Day, BigDecimal? Offset = null, BigDatePlanet? Planet = null)
        : this(Year, Month, Day, 0, 0, 0, Offset, Planet)
    {
    }
    public BigDateTimeOffset(BigDecimal TotalSeconds, BigDecimal? Offset = null, BigDatePlanet? Planet = null)
        : this(0, 1, 1, 0, 0, TotalSeconds, Offset, Planet)
    {
    }
    public BigDateTimeOffset(DateTimeOffset DateTimeOffset)
        : this(DateTimeOffset.Year, DateTimeOffset.Month, DateTimeOffset.Day, DateTimeOffset.Hour, DateTimeOffset.Minute, DateTimeOffset.Second + (BigDecimal)DateTimeOffset.Millisecond / 1000, DateTimeOffset.Offset.TotalSeconds)
    {
    }

    public BigInteger Year => BigDateTime.Year;
    public BigInteger Month => BigDateTime.Month;
    public BigInteger Day => BigDateTime.Day;
    public BigInteger Hour => BigDateTime.Hour;
    public BigInteger Minute => BigDateTime.Minute;
    public BigDecimal Second => BigDateTime.Second;
    public BigDatePlanet Planet => BigDateTime.Planet;

    public BigDateTimeOffset AddYears(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddYears(Value), Offset);
    }
    public BigDateTimeOffset AddMonths(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddMonths(Value), Offset);
    }
    public BigDateTimeOffset AddDays(BigInteger Value) {
        return new BigDateTimeOffset(BigDateTime.AddDays(Value), Offset);
    }
    public BigDateTimeOffset AddHours(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddHours(Value), Offset);
    }
    public BigDateTimeOffset AddMinutes(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddMinutes(Value), Offset);
    }
    public BigDateTimeOffset AddSeconds(BigDecimal Value) {
        return new BigDateTimeOffset(BigDateTime.AddSeconds(Value), Offset);
    }
    public BigDateTimeOffset AddMilliseconds(BigDecimal Value) {
        return AddSeconds(Value / 1000);
    }
    public BigDateTimeOffset AddMicroseconds(BigDecimal Value) {
        return AddMilliseconds(Value / 1000);
    }
    public BigDateTimeOffset AddNanoseconds(BigDecimal Value) {
        return AddMicroseconds(Value / 1000);
    }
    public BigDateTimeOffset Add(BigDateTimeOffset Value) {
        return AddSeconds(Value.TotalSeconds());
    }
    public BigDecimal Subtract(BigDateTimeOffset Value) {
        return TotalSeconds() - Value.TotalSeconds();
    }
    public BigDateTimeOffset AddOffset(BigDecimal Seconds) {
        return new BigDateTimeOffset(BigDateTime, Offset + Seconds);
    }
    public BigDateTimeOffset WithOffset(BigDecimal Seconds) {
        return new BigDateTimeOffset(BigDateTime, Seconds);
    }
    public BigDecimal TotalSeconds() {
        return BigDateTime.TotalSeconds() + Offset;
    }
    public BigInteger DaysInMonth() {
        return BigDateTime.AddSeconds(Offset).DaysInMonth();
    }
    public BigInteger DayOfYear() {
        return BigDateTime.AddSeconds(Offset).DayOfYear();
    }
    public BigInteger DayOfWeek() {
        return BigDateTime.AddSeconds(Offset).DayOfWeek();
    }
    public string ToString(string Format) {
        return BigDateTime.ToString(Format)
            .Format('z', (Offset >= 0 ? "+" : "-") + TimeSpan.FromSeconds((double)Offset).ToString("hh':'mm"));
    }
    public override string ToString() {
        return BigDateTime.ToString() + ToString(" z");
    }
    public string ToLongString() {
        return BigDateTime.ToLongString() + ToString(" z");
    }
    public string ToShortString() {
        return BigDateTime.ToShortString() + ToString( "z");
    }
    public int CompareTo(BigDateTimeOffset Other) {
        return TotalSeconds().CompareTo(Other.TotalSeconds());
    }
    public int CompareTo(object? Other) {
        return Other is BigDateTimeOffset OtherBigDateTimeOffset ? CompareTo(OtherBigDateTimeOffset) : 1;
    }

    public static BigDateTimeOffset Parse(string String, BigDatePlanet? Planet = null) {
        Planet ??= BigDatePlanet.Default;
        List<BigDecimal> Components = String.ParseDateComponents(9);
        return new BigDateTimeOffset(
            Components[0].WholeValue,
            Components[1].WholeValue,
            Components[2].WholeValue,
            Components[3],
            Components[4],
            Components[5],
            Components[6] * Planet.SecondsInHour + Components[7] * Planet.SecondsInMinute + Components[8],
            Planet
        );
    }
    public static bool TryParse(string String, out BigDateTimeOffset Result, BigDatePlanet? Planet = null) {
        try {
            Result = Parse(String, Planet);
            return true;
        }
        catch (Exception) {
            Result = default;
            return false;
        }
    }
    public static BigDateTimeOffset CurrentUniversalTime() {
        return DateTimeOffset.UtcNow;
    }
    public static BigDateTimeOffset CurrentLocalTime() {
        return DateTimeOffset.Now;
    }
    public static BigDateTimeOffset operator +(BigDateTimeOffset This, BigDateTimeOffset Other) {
        return This.Add(Other);
    }
    public static BigDecimal operator -(BigDateTimeOffset This, BigDateTimeOffset Other) {
        return This.Subtract(Other);
    }
    public static explicit operator DateTimeOffset(BigDateTimeOffset BigDateTimeOffset) {
        return new DateTimeOffset((DateTime)BigDateTimeOffset.BigDateTime, TimeSpan.FromSeconds((double)BigDateTimeOffset.Offset));
    }
    public static implicit operator BigDateTimeOffset(DateTimeOffset DateTimeOffset) {
        return new BigDateTimeOffset(DateTimeOffset.DateTime, DateTimeOffset.Offset.TotalSeconds);
    }
}