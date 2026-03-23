namespace LoveJourney.Domain.Entities;

public class Anniversary
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Recurrence { get; set; } = "none"; // none, monthly, yearly
    public int ReminderDaysBefore { get; set; } = 1;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Couple Couple { get; set; } = null!;
}
