using System.CommandLine;
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
        rootCommand.SetHandler(HandleRootCommand);

        foreach (Command command in serviceProvider.GetServices<Command>())
        {
            rootCommand.AddCommand(command);
        }

        rootCommand.Invoke(args);
    }

    private static void HandleRootCommand()
    {
        List<string> header = ["id", "name", "due", "pri"];
        List<List<string>> content =
        [
            ["1", "todo113", "", "l"],
            ["222", "todo2", "", "m"],
            ["3", "tod123123o32", "", "asdh"],
        ];

        Console.WriteLine(new TableUIFactory(header, content).Create());
    }
}
