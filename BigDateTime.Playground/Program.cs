﻿using ExtendedNumerics;

Console.WriteLine(BigDateTime.Now());
Console.WriteLine((DateTime)BigDateTime.Now());

Console.WriteLine(BigDateTime.Now().ToString("yyyy/MM/dd tt"));

Console.WriteLine(new BigDateTime(2023, 10, 24));
Console.WriteLine(new BigDateTime(2023, 10, 24).AddYears(1));
Console.WriteLine(new BigDateTime(2023, 10, 24).AddYears(2));
Console.WriteLine(new BigDateTime(2023, 10, 24).AddYears(2245903486503946));

Console.WriteLine(new BigDateTime(2023, 10, 24));
Console.WriteLine(new BigDateTime(2023, 10, 24).AddMonths(1));
Console.WriteLine(new BigDateTime(2023, 10, 24).AddMonths(2));
Console.WriteLine(new BigDateTime(2023, 10, 24).AddMonths(24));
Console.WriteLine(new BigDateTime(2024, 2, 29).AddMonths(12));

Console.WriteLine(new BigDateTime(-5, 1, 3));
Console.WriteLine(new BigDateTime(0, 2, 3));

Console.WriteLine(BigDateTime.Parse("2012/2/28"));
Console.WriteLine(new BigDateTime(2012, 2, 28));

Console.WriteLine(new BigDateTime(2013, 2, 28));

Console.WriteLine(new BigDateTime(2016, 2, 28));

Console.WriteLine(BigDateTime.Parse("2023/2/2").DayOfWeekName() + " -> " + BigDateTime.Parse("2023/2/3").DayOfWeekName());
Console.WriteLine(DateTime.Parse("2023/2/2").DayOfWeek + " -> " + DateTime.Parse("2023/2/3").DayOfWeek);

Console.WriteLine(new BigDateTime(60_000, 12, 30));
Console.WriteLine(new BigDateTime(60_000, 12, 30).ToShortString());
Console.WriteLine(new BigDateTime(60_000, 12, 30).ToLongString());

Console.WriteLine(DateTime.Parse("2023/1/2").ToString("yyy"));
Console.WriteLine(BigDateTime.Parse("2023/1/2").ToString("yyy/MM/dd hh:mm:ss"));

Console.WriteLine(new BigDateTime(60_000, 12, 30));
Console.WriteLine(new BigDateTime(60_000, 12, 30).AddDays(1));
Console.WriteLine(new BigDateTime(60_000, 12, 30).AddDays(2));

Console.WriteLine(new BigDateTimeOffset(451934750934));
Console.WriteLine(new BigDateTimeOffset(451934750934).AddDays(2));

Console.WriteLine(BigDateTime.Now(0).ToLongString());
Console.WriteLine(BigDateTime.Now().ToLongString());

Console.WriteLine(new DateTime(2024, 08, 11, 01, 34, 00).Subtract(DateTime.MinValue).TotalSeconds);
Console.WriteLine(new BigDateTime(2024, 08, 11, 01, 34, 00).Subtract(DateTime.MinValue));

Console.WriteLine(DateTimeOffset.Now);
Console.WriteLine(BigDateTimeOffset.Now());

Console.WriteLine(DateTimeOffset.Now.DayOfWeek);
Console.WriteLine(BigDateTime.Now().DayOfWeek);

Console.WriteLine();
Console.WriteLine("Realtime:");
Console.CursorVisible = false;
while (true) {
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(BigDateTime.Now().ToLongString().PadRight(50));
    Thread.Sleep(TimeSpan.FromSeconds(0.05));
}