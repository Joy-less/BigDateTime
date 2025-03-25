namespace ExtendedNumerics.Tests;

public class BigDateTimeTests {
    [Fact]
    public void Test1() {
        BigDateTime SciFiDate = new(60_000, 12, 30);
        SciFiDate.Year.ShouldBe(60_000);
        SciFiDate.Month.ShouldBe(12);
        SciFiDate.Day.ShouldBe(30);
        SciFiDate.Hour.ShouldBe(0);
        SciFiDate.Minute.ShouldBe(0);
        SciFiDate.Second.ShouldBe(0);
    }
    [Fact]
    public void Test2() {
        DateTime DateTime = new(2024, 08, 11, 01, 34, 00);
        BigDateTime BigDateTime = new(2024, 08, 11, 01, 34, 00);
        (BigDateTime - new BigDateTime(1, 1, 1)).ShouldBe((DateTime - DateTime.MinValue).TotalSeconds);
    }
    [Fact]
    public void Test3() {
        BigDateTime.Now().Year.ShouldBe(DateTime.Now.Year);
        BigDateTime.Now().Month.ShouldBe(DateTime.Now.Month);
        BigDateTime.Now().Day.ShouldBe(DateTime.Now.Day);
        BigDateTime.Now().Hour.ShouldBe(DateTime.Now.Hour);
        BigDateTime.Now().Minute.ShouldBe(DateTime.Now.Minute);
        BigReal.GetWholePart(BigDateTime.Now().Second).ShouldBe(DateTime.Now.Second);
    }
    [Fact]
    public void Test4() {
        BigDateTimeOffset.Now().Year.ShouldBe(DateTimeOffset.Now.Year);
        BigDateTimeOffset.Now().Month.ShouldBe(DateTimeOffset.Now.Month);
        BigDateTimeOffset.Now().Day.ShouldBe(DateTimeOffset.Now.Day);
        BigDateTimeOffset.Now().Hour.ShouldBe(DateTimeOffset.Now.Hour);
        BigDateTimeOffset.Now().Minute.ShouldBe(DateTimeOffset.Now.Minute);
        BigReal.GetWholePart(BigDateTimeOffset.Now().Second).ShouldBe(DateTimeOffset.Now.Second);
    }
    [Fact]
    public void Test5() {
        BigDateTime.Now().ToString("yyyy-MM-dd hh-mm-ss").ShouldBe(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
    }
    [Fact]
    public void Test6() {
        BigDateTimeOffset.Now(0).ToString("yyyy-MM-dd hh-mm-ss zzz").ShouldBe(DateTimeOffset.UtcNow.ToString("yyyy-MM-dd hh-mm-ss zzz"));
    }
    [Fact]
    public void Test7() {
        BigDateTimeOffset.Parse("2025/01/09").ShouldBe(DateTimeOffset.Parse("2025-01-09"));
    }
}