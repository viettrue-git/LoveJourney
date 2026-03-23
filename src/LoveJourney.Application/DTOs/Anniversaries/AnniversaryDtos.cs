namespace LoveJourney.Application.DTOs.Anniversaries;

public class CreateAnniversaryRequest
{
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Recurrence { get; set; } = "none";
    public int ReminderDaysBefore { get; set; } = 1;
    public string? Notes { get; set; }
}

public class UpdateAnniversaryRequest
{
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Recurrence { get; set; } = "none";
    public int ReminderDaysBefore { get; set; } = 1;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}

public class AnniversaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Recurrence { get; set; } = string.Empty;
    public int ReminderDaysBefore { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? NextOccurrence { get; set; }
    public int? DaysUntilNext { get; set; }
    public DateTime CreatedAt { get; set; }
}
