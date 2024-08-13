namespace ExtendedNumerics.Tests;

[TestClass]
public class BigDateTests {
    [TestMethod]
    public void Test1() {
        BigDateTime SciFiDate = new(60_000, 12, 30);
        Assert.AreEqual(SciFiDate.Year, 60_000);
        Assert.AreEqual(SciFiDate.Month, 12);
        Assert.AreEqual(SciFiDate.Day, 30);
        Assert.AreEqual(SciFiDate.Hour, 0);
        Assert.AreEqual(SciFiDate.Minute, 0);
        Assert.AreEqual(SciFiDate.Second, 0);
    }
    [TestMethod]
    public void Test2() {
        DateTime DateTime = new(2024, 08, 11, 01, 34, 00);
        BigDateTime BigDateTime = new(2024, 08, 11, 01, 34, 00);
        Assert.AreEqual(BigDateTime - new BigDateTime(1, 1, 1), (DateTime - DateTime.MinValue).TotalSeconds);
        Assert.AreEqual(BigDateTime.TotalSeconds() - new BigDatePlanet().SecondsInYear, (DateTime - DateTime.MinValue).TotalSeconds);
    }
    [TestMethod]
    public void Test3() {
        Assert.AreEqual(DateTime.Now.Year, BigDateTime.CurrentLocalTime().Year);
        Assert.AreEqual(DateTime.Now.Month, BigDateTime.CurrentLocalTime().Month);
        Assert.AreEqual(DateTime.Now.Day, BigDateTime.CurrentLocalTime().Day);
        Assert.AreEqual(DateTime.Now.Hour, BigDateTime.CurrentLocalTime().Hour);
        Assert.AreEqual(DateTime.Now.Minute, BigDateTime.CurrentLocalTime().Minute);
        Assert.AreEqual(DateTime.Now.Second, BigDateTime.CurrentLocalTime().Second.GetWholePart());
    }
    [TestMethod]
    public void Test4() {
        Assert.AreEqual(DateTimeOffset.Now.Year, BigDateTimeOffset.CurrentLocalTime().Year);
        Assert.AreEqual(DateTimeOffset.Now.Month, BigDateTimeOffset.CurrentLocalTime().Month);
        Assert.AreEqual(DateTimeOffset.Now.Day, BigDateTimeOffset.CurrentLocalTime().Day);
        Assert.AreEqual(DateTimeOffset.Now.Hour, BigDateTimeOffset.CurrentLocalTime().Hour);
        Assert.AreEqual(DateTimeOffset.Now.Minute, BigDateTimeOffset.CurrentLocalTime().Minute);
        Assert.AreEqual(DateTimeOffset.Now.Second, BigDateTimeOffset.CurrentLocalTime().Second.GetWholePart());
    }
    [TestMethod]
    public void Test5() {
        Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"), BigDateTime.CurrentLocalTime().ToString("yyyy-MM-dd hh-mm-ss"));
    }
    [TestMethod]
    public void Test6() {
        Assert.AreEqual(DateTimeOffset.UtcNow.ToString("yyyy-MM-dd hh-mm-ss zzzz"), BigDateTimeOffset.CurrentUniversalTime().ToString("yyyy-MM-dd hh-mm-ss zzzz"));
    }
}