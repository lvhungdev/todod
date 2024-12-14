using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Daemon;

public static class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddDaemon(builder.Configuration)
            .AddHostedService<NotificationWorker>();

        builder.Build().Run();
    }
}
