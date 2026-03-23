using FluentValidation;
using LoveJourney.Application.DTOs.Reviews;

namespace LoveJourney.Application.Validators;

public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween((short)1, (short)5)
            .WithMessage("Đánh giá phải từ 1 đến 5.");
        RuleFor(x => x.ReviewText).MaximumLength(2000);
        RuleFor(x => x.Tips).MaximumLength(1000);
    }
}
