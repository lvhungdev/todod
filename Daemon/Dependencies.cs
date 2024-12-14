using System.Runtime.InteropServices;
using Adapter.Notification;
using Adapter.Persistence;
using Application.Ports;
using Application.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Daemon;

public static class Dependencies
{
    public static IServiceCollection AddDaemon(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext(configuration)
            .AddNotificationService()
            .AddScoped<NotificationUseCases>();
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new NullReferenceException("Connection string not found");

        return services
            .AddDbContext<AppDbContext>(options => options.UseSqlite(connString))
            .AddScoped<IAppDbContext, AppDbContext>();
    }

    private static IServiceCollection AddNotificationService(this IServiceCollection services)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            services.AddScoped<INotificationService, OsxNotificationService>();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
        }

        return services;
    }
}
