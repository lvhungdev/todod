using Application.Entities;

namespace ConsoleApp.UI;

public class PriorityUIFactory
{
    private readonly Priority _priority;

    public PriorityUIFactory(Priority priority)
    {
        _priority = priority;
    }

    public string Create()
    {
        return _priority switch
        {
            Priority.Low => "L",
            Priority.Medium => "M",
            Priority.High => "H",
            _ => "",
        };
    }
}
