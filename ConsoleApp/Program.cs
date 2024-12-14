using System.CommandLine;
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

        rootCommand.Invoke(args);
    }
}
