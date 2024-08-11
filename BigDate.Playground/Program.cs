using BigDate;

Console.WriteLine(new BigDateTime(60_000, 12, 30));
Console.WriteLine(new BigDateTime(60_000, 12, 30).ToShortString());
Console.WriteLine(new BigDateTime(60_000, 12, 30).ToLongString());

Console.WriteLine(DateTime.Parse("2023/1/2").ToString("yyy"));
Console.WriteLine(BigDateTime.Parse("2023/1/2").ToString("yyy/MM/dd hh:mm:ss"));

Console.WriteLine(new BigDateTime(60_000, 12, 30));
Console.WriteLine(new BigDateTime(60_000, 12, 30).AddDays(1));
Console.WriteLine(new BigDateTime(60_000, 12, 30).AddDays(2));

Console.WriteLine(new BigDateTime(451934750934));
Console.WriteLine(new BigDateTime(451934750934).AddDays(2));

Console.WriteLine(BigDateTime.CurrentUniversalTime().ToLongString());
Console.WriteLine(BigDateTime.CurrentLocalTime().ToLongString());

Console.WriteLine(new DateTime(2024, 08, 11, 01, 34, 00).Subtract(DateTime.MinValue).TotalSeconds);
Console.WriteLine(new BigDateTime(2024, 08, 11, 01, 34, 00).Subtract(DateTime.MinValue));

Console.WriteLine(DateTimeOffset.Now);
Console.WriteLine(BigDateTimeOffset.CurrentLocalTime());