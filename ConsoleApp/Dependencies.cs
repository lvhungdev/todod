using System.CommandLine;
using Adapter.Persistence;
using Application.Ports;
using Application.UseCases;
using ConsoleApp.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp;

public static class Dependencies
{
    public static IServiceCollection AddConsoleApp(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext(configuration)
            .AddLogging(configure =>
            {
                configure.AddConfiguration(configuration.GetSection("Logging"));
                configure.AddConsole();
            })
            .AddScoped<TodoUseCases>()
            .AddCommands();
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new NullReferenceException("Connection string not found");
        return services
            .AddDbContext<AppDbContext>(options => options.UseSqlite(connString))
            .AddScoped<IAppDbContext, AppDbContext>();
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services
            .AddScoped<Command, ListCommand>()
            .AddScoped<Command, AddCommand>()
            .AddScoped<Command, CompleteCommand>()
            .AddScoped<Command, ModifyCommand>();
    }
}
