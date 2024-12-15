using System.CommandLine;
using Application.Entities;
using Application.UseCases;
using FluentResults;

namespace ConsoleApp.Commands;

public class CompleteCommand : Command
{
    private readonly TodoUseCases _todoUseCases;

    public CompleteCommand(TodoUseCases todoUseCases) : base("cmp", "Complete a todo")
    {
        _todoUseCases = todoUseCases;
        Setup();
    }

    private void Setup()
    {
        Argument<int> idArg = new("id", "Todo id");
        AddArgument(idArg);

        this.SetHandler(Handle, idArg);
    }

    private async Task Handle(int id)
    {
        List<Todo> todos = await _todoUseCases.GetAllActive();
        if (id < 1 || id > todos.Count)
        {
            Console.WriteLine($"todo with id {id} could not be found");
            return;
        }

        Result<Todo> result = await _todoUseCases.Complete(todos[id - 1].Id);

        Console.WriteLine(
            result.IsFailed
                ? result.Errors.First().Message
                : $"completed {todos[id - 1].Name}");
    }
}
