# BigDateTime

[![NuGet](https://img.shields.io/nuget/v/BigDateTime.svg)](https://www.nuget.org/packages/BigDateTime)

An arbitrary size and precision date and time stored using a `BigReal`, that can represent any moment, even `60242/1/5 23:59:22`.

The first of its kind to my knowledge.

## Features

- Supports arbitrary-size dates (`DateTime` only supports years 1 to 9999)
- Supports arbitrary-precision times (`DateTime` is only precise to 100 nanoseconds / 0.0000001 seconds)
- Highly performant (uses black magic algorithms from `DateTime`)

## Examples

### 100,000 Years From Now

```cs
Console.WriteLine(BigDateTime.Now().AddYears(100_000)); // 102024/10/24 23:59:44
```

### Realtime Clock

```cs
Console.CursorVisible = false;
while (true) {
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(BigDateTime.Now().ToLongString().PadRight(50));
    Thread.Sleep(TimeSpan.FromSeconds(0.05));
}
```