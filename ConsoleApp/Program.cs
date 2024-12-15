using System.CommandLine;
using Application.Entities;
using Application.UseCases;
using ConsoleApp.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        ServiceCollection serviceCollection = new();
        serviceCollection.AddConsoleApp(configuration);

        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        RootCommand rootCommand = new();

        foreach (Command command in serviceProvider.GetServices<Command>())
        {
            rootCommand.AddCommand(command);
        }

        rootCommand.SetHandler(async () => await HandleRootCommand(serviceProvider.GetRequiredService<TodoUseCases>()));

        rootCommand.Invoke(args);
    }

    private static async Task HandleRootCommand(TodoUseCases todoUseCases)
    {
        List<Todo> todos = await todoUseCases.GetAllActive();
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

        Console.WriteLine(new TableUIFactory(header, content).Create());
    }
}
