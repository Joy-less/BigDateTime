namespace ExtendedNumerics;

[TestClass]
public class BigDateTests {
    [TestMethod]
    public void Test1() {
        BigDateTime SciFiDate = new(60_000, 12, 30);
        Assert.AreEqual(60_000, SciFiDate.Year);
        Assert.AreEqual(12, SciFiDate.Month);
        Assert.AreEqual(30, SciFiDate.Day);
        Assert.AreEqual(0, SciFiDate.Hour);
        Assert.AreEqual(0, SciFiDate.Minute);
        Assert.AreEqual(0, SciFiDate.Second);
    }
    [TestMethod]
    public void Test2() {
        DateTime DateTime = new(2024, 08, 11, 01, 34, 00);
        BigDateTime BigDateTime = new(2024, 08, 11, 01, 34, 00);
        Assert.AreEqual((DateTime - DateTime.MinValue).TotalSeconds, BigDateTime - new BigDateTime(1, 1, 1));
        Assert.AreEqual((DateTime - DateTime.MinValue).TotalSeconds, BigDateTime.TotalSeconds - Planet.Earth.SecondsInCommonYear);
    }
    [TestMethod]
    public void Test3() {
        Assert.AreEqual(DateTime.Now.Year, new BigDateTime(2024, 08, 16, 02, 06, 00).Year);
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
        Assert.AreEqual(DateTimeOffset.UtcNow.ToString("yyyy-MM-dd hh-mm-ss zzz"), BigDateTimeOffset.CurrentUniversalTime().ToString("yyyy-MM-dd hh-mm-ss zzz"));
    }
}