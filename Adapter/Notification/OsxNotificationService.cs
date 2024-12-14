using System.Diagnostics;
using Application.Ports;
using FluentResults;

namespace Adapter.Notification;

public class OsxNotificationService : INotificationService
{
    public Result Notify(string title, string message)
    {
        string script = $"display notification \\\"{message}\\\" with title \\\"{title}\\\"";

        ProcessStartInfo processInfo = new()
        {
            FileName = "osascript",
            Arguments = $"-e \"{script}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };


        using Process process = new();
        process.StartInfo = processInfo;
        process.Start();

        process.WaitForExit();

        string error = process.StandardError.ReadToEnd();

        return error == "" ? Result.Ok() : Result.Fail(error);
    }
}
