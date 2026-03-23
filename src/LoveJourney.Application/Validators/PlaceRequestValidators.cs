using FluentValidation;
using LoveJourney.Application.DTOs.Places;

namespace LoveJourney.Application.Validators;

public class CreatePlaceRequestValidator : AbstractValidator<CreatePlaceRequest>
{
    private static readonly string[] ValidCategories = { "restaurant", "hotel", "attraction", "cafe", "other" };

    public CreatePlaceRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.Latitude).InclusiveBetween(-90m, 90m).When(x => x.Latitude.HasValue);
        RuleFor(x => x.Longitude).InclusiveBetween(-180m, 180m).When(x => x.Longitude.HasValue);
        RuleFor(x => x.Category).Must(c => c == null || ValidCategories.Contains(c))
            .WithMessage("Loại địa điểm không hợp lệ.");
    }
}

public class UpdatePlaceRequestValidator : AbstractValidator<UpdatePlaceRequest>
{
    private static readonly string[] ValidCategories = { "restaurant", "hotel", "attraction", "cafe", "other" };

    public UpdatePlaceRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.Latitude).InclusiveBetween(-90m, 90m).When(x => x.Latitude.HasValue);
        RuleFor(x => x.Longitude).InclusiveBetween(-180m, 180m).When(x => x.Longitude.HasValue);
        RuleFor(x => x.Category).Must(c => c == null || ValidCategories.Contains(c))
            .WithMessage("Loại địa điểm không hợp lệ.");
    }
}
