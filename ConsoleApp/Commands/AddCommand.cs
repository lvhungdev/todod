using System.CommandLine;
using Application.Entities;
using Application.UseCases;
using ConsoleApp.Parsers;
using FluentResults;

namespace ConsoleApp.Commands;

public class AddCommand : Command
{
    private readonly TodoUseCases _todoUseCases;

    public AddCommand(TodoUseCases todoUseCases) : base("add", "Add a new todo")
    {
        _todoUseCases = todoUseCases;

        Setup();
    }

    private void Setup()
    {
        Argument<string> nameArg = new("name", "Todo name");
        AddArgument(nameArg);

        Option<string?> dueDateOpt = new(["-d", "--due"], "Due date");
        AddOption(dueDateOpt);

        Option<string?> priorityOpt = new(["-p", "--pri"], "Priority");
        AddOption(priorityOpt);

        this.SetHandler(Handle, nameArg, dueDateOpt, priorityOpt);
    }

    private async Task Handle(string name, string? dueDate, string? priority)
    {
        DateTime due = default;
        if (dueDate != null)
        {
            Result<DateTime> dateParseResult = TimeParser.Parse(dueDate);
            if (dateParseResult.IsFailed)
            {
                Console.WriteLine(dateParseResult.Errors.First().Message);
                return;
            }

            due = dateParseResult.Value;
        }

        Priority pri = default;
        if (priority != null)
        {
            Result<Priority> priorityParseResult = PriorityParser.Parse(priority);
            if (priorityParseResult.IsFailed)
            {
                Console.WriteLine(priorityParseResult.Errors.First().Message);
                return;
            }

            pri = priorityParseResult.Value;
        }

        Result<Todo> result = await _todoUseCases.Add(name, dueDate: due, priority: pri);

        Console.WriteLine(
            result.IsFailed
                ? result.Errors.First().ToString()
                : $"Added {result.Value.Name}");
    }
}
