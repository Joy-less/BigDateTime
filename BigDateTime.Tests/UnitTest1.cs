namespace ExtendedNumerics.Tests;

public class BigDateTests {
    [Fact]
    public void Test1() {
        BigDateTime SciFiDate = new(60_000, 12, 30);
        Assert.Equal(60_000, SciFiDate.Year);
        Assert.Equal(12, SciFiDate.Month);
        Assert.Equal(30, SciFiDate.Day);
        Assert.Equal(0, SciFiDate.Hour);
        Assert.Equal(0, SciFiDate.Minute);
        Assert.Equal(0, SciFiDate.Second);
    }
    [Fact]
    public void Test2() {
        DateTime DateTime = new(2024, 08, 11, 01, 34, 00);
        BigDateTime BigDateTime = new(2024, 08, 11, 01, 34, 00);
        Assert.Equal((DateTime - DateTime.MinValue).TotalSeconds, BigDateTime - new BigDateTime(1, 1, 1));
    }
    [Fact]
    public void Test3() {
        Assert.Equal(DateTime.Now.Year, BigDateTime.Now().Year);
        Assert.Equal(DateTime.Now.Month, BigDateTime.Now().Month);
        Assert.Equal(DateTime.Now.Day, BigDateTime.Now().Day);
        Assert.Equal(DateTime.Now.Hour, BigDateTime.Now().Hour);
        Assert.Equal(DateTime.Now.Minute, BigDateTime.Now().Minute);
        Assert.Equal(DateTime.Now.Second, BigDateTime.Now().Second.GetWholePart());
    }
    [Fact]
    public void Test4() {
        Assert.Equal(DateTimeOffset.Now.Year, BigDateTimeOffset.Now().Year);
        Assert.Equal(DateTimeOffset.Now.Month, BigDateTimeOffset.Now().Month);
        Assert.Equal(DateTimeOffset.Now.Day, BigDateTimeOffset.Now().Day);
        Assert.Equal(DateTimeOffset.Now.Hour, BigDateTimeOffset.Now().Hour);
        Assert.Equal(DateTimeOffset.Now.Minute, BigDateTimeOffset.Now().Minute);
        Assert.Equal(DateTimeOffset.Now.Second, BigDateTimeOffset.Now().Second.GetWholePart());
    }
    [Fact]
    public void Test5() {
        Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"), BigDateTime.Now().ToString("yyyy-MM-dd hh-mm-ss"));
    }
    [Fact]
    public void Test6() {
        Assert.Equal(DateTimeOffset.UtcNow.ToString("yyyy-MM-dd hh-mm-ss zzz"), BigDateTimeOffset.Now(0).ToString("yyyy-MM-dd hh-mm-ss zzz"));
    }
    [Fact]
    public void Test7() {
        Assert.Equal(BigDateTimeOffset.Parse("2025/01/09"), DateTimeOffset.Parse("2025-01-09"));
    }
}