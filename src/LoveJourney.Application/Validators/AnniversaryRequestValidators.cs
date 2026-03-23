using FluentValidation;
using LoveJourney.Application.DTOs.Anniversaries;

namespace LoveJourney.Application.Validators;

public class CreateAnniversaryRequestValidator : AbstractValidator<CreateAnniversaryRequest>
{
    private static readonly string[] ValidRecurrences = { "none", "monthly", "yearly" };

    public CreateAnniversaryRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Recurrence).Must(r => ValidRecurrences.Contains(r))
            .WithMessage("Kiểu lặp không hợp lệ.");
        RuleFor(x => x.ReminderDaysBefore).InclusiveBetween(0, 30);
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}

public class UpdateAnniversaryRequestValidator : AbstractValidator<UpdateAnniversaryRequest>
{
    private static readonly string[] ValidRecurrences = { "none", "monthly", "yearly" };

    public UpdateAnniversaryRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Recurrence).Must(r => ValidRecurrences.Contains(r))
            .WithMessage("Kiểu lặp không hợp lệ.");
        RuleFor(x => x.ReminderDaysBefore).InclusiveBetween(0, 30);
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}
