using Application.Entities;
using FluentResults;

namespace ConsoleApp.Parsers;

public static class PriorityParser
{
    public static Result<Priority> Parse(string input)
    {
        return input switch
        {
            "low" => Priority.Low,
            "medium" => Priority.Medium,
            "high" => Priority.High,
            _ => Result.Fail($"unknown priority {input}"),
        };
    }
}
