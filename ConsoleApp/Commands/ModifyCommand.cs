using System.CommandLine;
using Application.Entities;
using Application.UseCases;
using ConsoleApp.Parsers;
using FluentResults;

namespace ConsoleApp.Commands;

public class ModifyCommand : Command
{
    private readonly TodoUseCases _todoUseCases;

    public ModifyCommand(TodoUseCases todoUseCases) : base("mod", "Modify a todo")
    {
        _todoUseCases = todoUseCases;
        Setup();
    }

    private void Setup()
    {
        Argument<int> idArg = new("id", "Todo id");
        AddArgument(idArg);

        Option<string?> nameOpt = new(["-n", "--name"], "Todo name");
        AddOption(nameOpt);

        Option<string?> dueDateOpt = new(["-d", "--due"], "Due date");
        AddOption(dueDateOpt);

        Option<string?> priorityOpt = new(["-p", "--pri"], "Priority");
        AddOption(priorityOpt);

        this.SetHandler(Handle, idArg, nameOpt, dueDateOpt, priorityOpt);
    }

    private async Task Handle(int id, string? nameOpt, string? dueDateOpt, string? priorityOpt)
    {
        List<Todo> todos = await _todoUseCases.GetAllActive();
        if (id < 1 || id > todos.Count)
        {
            Console.WriteLine($"todo with id {id} could not be found");
            return;
        }

        DateTime? dueDate = null;
        if (dueDateOpt != null)
        {
            Result<DateTime> dateParseResult = TimeParser.Parse(dueDateOpt);
            if (dateParseResult.IsFailed)
            {
                Console.WriteLine(dateParseResult.Errors.First().Message);
                return;
            }

            dueDate = dateParseResult.Value;
        }

        Priority? priority = null;
        if (priorityOpt != null)
        {
            Result<Priority> priorityParseResult = PriorityParser.Parse(priorityOpt);
            if (priorityParseResult.IsFailed)
            {
                Console.WriteLine(priorityParseResult.Errors.First().Message);
                return;
            }

            priority = priorityParseResult.Value;
        }

        Result<Todo> result = await _todoUseCases.Update(todos[id - 1].Id, nameOpt, dueDate, priority);

        Console.WriteLine(
            result.IsFailed
                ? result.Errors.First().Message
                : $"modified {todos[id - 1].Name}");
    }
}
