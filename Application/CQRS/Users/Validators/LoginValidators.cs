using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.CQRS.Users.Validators;

public class LoginValidators : AbstractValidator<Login.LoginRequest>
{
    public LoginValidators()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50);
    }
}
