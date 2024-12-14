using Application.Entities;
using Application.Ports;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases;

public class NotificationUseCases
{
    private readonly IAppDbContext _dbContext;
    private readonly INotificationService _notificationService;

    public NotificationUseCases(IAppDbContext dbContext, INotificationService notificationService)
    {
        _dbContext = dbContext;
        _notificationService = notificationService;
    }

    public async Task<Result> SendNotificationsForDueTodos()
    {
        List<Todo> todos = await _dbContext.Todos
            .Where(m => m.CompletedDate == default && m.DueDate != default && m.DueDate <= DateTime.Now &&
                        !m.IsNotified)
            .ToListAsync();

        if (todos.Count == 0) return Result.Ok();

        todos = todos.OrderByDescending(m => m.GetUrgency()).ToList();

        bool hasMultipleTodos = todos.Count > 1;

        string title = hasMultipleTodos ? "You have due todos" : "You have a due todo";
        string message = hasMultipleTodos
            ? $"{todos.First().Name} and others are due."
            : $"{todos.First().Name} is due.";

        Result result = _notificationService.Notify(title, message);
        if (result.IsFailed) return result;

        foreach (Todo todo in todos)
        {
            todo.IsNotified = true;
        }

        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}
