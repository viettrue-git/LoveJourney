using FluentValidation;
using LoveJourney.Application.DTOs.Journeys;

namespace LoveJourney.Application.Validators;

public class CreateJourneyRequestValidator : AbstractValidator<CreateJourneyRequest>
{
    private static readonly string[] ValidTypes = { "trip", "dining", "activity", "other" };

    public CreateJourneyRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.JourneyType).Must(t => ValidTypes.Contains(t))
            .WithMessage("Loại hành trình không hợp lệ.");
        RuleFor(x => x.JourneyDate).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.JourneyDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu.");
    }
}

public class UpdateJourneyRequestValidator : AbstractValidator<UpdateJourneyRequest>
{
    private static readonly string[] ValidTypes = { "trip", "dining", "activity", "other" };

    public UpdateJourneyRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.JourneyType).Must(t => ValidTypes.Contains(t))
            .WithMessage("Loại hành trình không hợp lệ.");
        RuleFor(x => x.JourneyDate).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.JourneyDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu.");
    }
}
