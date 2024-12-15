namespace Application.Entities;

public class Todo
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public DateTime CompletedDate { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public bool IsNotified { get; set; }

    public float GetUrgency()
    {
        float urgency = 0;

        if (DueDate != default)
        {
            urgency += 0.2f;

            TimeSpan duration = DueDate - DateTime.Now;
            float durationInSeconds = Math.Max(0, 60 * 60 * 24 * 7 - (int)duration.TotalSeconds);

            const float urgencyPerDay = 1.0f;
            const float urgencyPerSecond = urgencyPerDay / (60 * 60 * 24);
            urgency += urgencyPerSecond * durationInSeconds;
        }

        switch (Priority)
        {
            case Priority.High:
                urgency += 4.0f;
                break;
            case Priority.Medium:
                urgency += 2.0f;
                break;
            case Priority.Low:
            default:
                break;
        }

        return urgency;
    }
}

public enum Priority
{
    Low,
    Medium,
    High,
}
