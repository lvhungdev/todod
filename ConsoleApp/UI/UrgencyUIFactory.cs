namespace ConsoleApp.UI;

public class UrgencyUIFactory
{
    private readonly float _urgency;

    public UrgencyUIFactory(float urgency)
    {
        _urgency = urgency;
    }

    public string Create()
    {
        return _urgency == 0 ? "" : _urgency.ToString("F2");
    }
}
