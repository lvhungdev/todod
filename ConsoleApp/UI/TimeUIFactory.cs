namespace ConsoleApp.UI;

public class TimeUIFactory
{
    private readonly DateTime _time;

    public TimeUIFactory(DateTime time)
    {
        _time = time;
    }

    public string CreateRelative()
    {
        if (_time == default) return "";

        DateTime now = DateTime.Now;
        TimeSpan timeSpan = now - _time;

        return timeSpan.TotalSeconds < 0
            ? Format(-(int)timeSpan.TotalSeconds, "")
            : Format((int)timeSpan.TotalSeconds, "-");
    }

    private static string Format(int seconds, string prefix)
    {
        return seconds switch
        {
            < 60 => $"{prefix}{seconds}s",
            < 3600 => $"{prefix}{seconds / 60}m{seconds % 60}s",
            < 86400 => $"{prefix}{seconds / 3600}h{seconds % 3600 / 60}m",
            _ => $"{prefix}{seconds / 86400}d{seconds % 86400 / 3600}h",
        };
    }
}
