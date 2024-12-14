using System.CommandLine;
using Application.Entities;
using Application.UseCases;

namespace ConsoleApp.Commands;

public class ListCommand : Command
{
    private readonly TodoUseCases _todoUseCases;

    public ListCommand(TodoUseCases todoUseCases) : base("list", "List all active todos")
    {
        _todoUseCases = todoUseCases;

        this.SetHandler(Handle);
    }

    private async Task Handle()
    {
        List<Todo> todos = await _todoUseCases.GetAllActive();
        Console.WriteLine(todos);
    }
}
