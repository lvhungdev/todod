using Application.UseCases;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Daemon;

public class NotificationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationWorker> _logger;

    public NotificationWorker(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationWorker> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                NotificationUseCases notificationUseCases =
                    scope.ServiceProvider.GetRequiredService<NotificationUseCases>();

                Result result = await notificationUseCases.SendNotificationsForDueTodos();
                if (result.IsFailed)
                {
                    _logger.LogError("Failed to send notifications for due tasks {error}",
                        result.Errors.First().Message);
                }

                await Task.Delay(1000 * 60, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An error occured: {error}", ex.Message);
            }
        }
    }
}
