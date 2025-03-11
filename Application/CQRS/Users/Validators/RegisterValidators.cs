using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.CQRS.Users.Validators;

public class RegisterValidators : AbstractValidator<Register.Command>
{
    public RegisterValidators()
    {
        RuleFor(u => u.Firstname)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(u => u.Lastname)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(u => u.Age)
            .NotEmpty()
            .GreaterThan(18);

        RuleFor(u => u.Gender)
            .NotEmpty()
            .MaximumLength(20);

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
