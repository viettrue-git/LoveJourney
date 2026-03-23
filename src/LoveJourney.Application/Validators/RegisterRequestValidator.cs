using FluentValidation;
using LoveJourney.Application.DTOs.Auth;

namespace LoveJourney.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email không hợp lệ.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");
        RuleFor(x => x.Partner1Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Partner2Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Ngày bắt đầu không thể ở tương lai.");
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class UpdateProfileRequestValidator : AbstractValidator<DTOs.Profile.UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Partner1Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Partner2Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}
