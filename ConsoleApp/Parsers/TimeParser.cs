using System.Globalization;
using FluentResults;

namespace ConsoleApp.Parsers;

public static class TimeParser
{
    public static Result<DateTime> Parse(string input)
    {
        DateTime? result = ParseAbsoluteTime(input) ?? ParseRelativeTime(input) ?? ParseEndOfTime(input);

        if (result == null)
        {
            return Result.Fail($"{input} was not a valid date format");
        }

        return result;
    }

    private static DateTime? ParseAbsoluteTime(string input)
    {
        string[] formats = ["yyyy-MM-ddTHH:mm:ss", "yyyy-MM-dd", "HH:mm:ss", "HH:mm"];

        bool isSuccess = DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTime date);

        return isSuccess ? date : null;
    }

    private static DateTime? ParseRelativeTime(string input)
    {
        if (input.Length < 2) return null;

        if (!int.TryParse(input[..^1], out int amount)) return null;
        string unit = input[^1..];

        return unit switch
        {
            "s" => DateTime.Now.AddSeconds(amount),
            "m" => DateTime.Now.AddMinutes(amount),
            "h" => DateTime.Now.AddHours(amount),
            "d" => DateTime.Now.AddDays(amount),
            "w" => DateTime.Now.AddDays(amount * 7),
            _ => null,
        };
    }

    private static DateTime? ParseEndOfTime(string input)
    {
        switch (input)
        {
            case "eod":
            {
                return DateTime.Today.AddDays(1).AddSeconds(-1);
            }
            case "eow":
            {
                int daysUntilEndOfWeek = ((int)DayOfWeek.Sunday - (int)DateTime.Now.DayOfWeek + 7) % 7;
                return DateTime.Today.AddDays(daysUntilEndOfWeek + 1).AddSeconds(-1);
            }
            case "eom":
            {
                DateTime now = DateTime.Now;
                int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
                return new DateTime(now.Year, now.Month, daysInMonth, 23, 59, 59);
            }
            case "eoy":
            {
                DateTime now = DateTime.Now;
                int daysInMonth = DateTime.DaysInMonth(now.Year, 12);
                return new DateTime(now.Year, 12, daysInMonth, 23, 59, 59);
            }
            default:
                return null;
        }
    }
}
