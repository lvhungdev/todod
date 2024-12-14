using FluentResults;

namespace Application.Ports;

public interface INotificationService
{
    Result Notify(string title, string message);
}
