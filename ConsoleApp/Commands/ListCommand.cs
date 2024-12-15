using System.CommandLine;
using Application.Entities;
using Application.UseCases;
using ConsoleApp.UI;

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
        if (todos.Count == 0)
        {
            Console.WriteLine("empty");
            return;
        }

        List<string> header = ["id", "name", "due", "pri", "urg"];
        List<List<string>> content = todos
            .Select((m, i) => new List<string>
            {
                (i + 1).ToString(),
                m.Name,
                new TimeUIFactory(m.DueDate).CreateRelative(),
                new PriorityUIFactory(m.Priority).Create(),
                new UrgencyUIFactory(m.GetUrgency()).Create(),
            })
            .ToList();
        List<TableUIRowAlign> rowAligns =
        [
            TableUIRowAlign.Right, TableUIRowAlign.Left, TableUIRowAlign.Right, TableUIRowAlign.Right,
            TableUIRowAlign.Right,
        ];

        Console.WriteLine(new TableUIFactory(header, content, rowAligns).Create());
    }
}
