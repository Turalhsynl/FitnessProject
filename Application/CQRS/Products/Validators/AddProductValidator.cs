using Application.CQRS.Products.Handlers;
using FluentValidation;

namespace Application.CQRS.Products.Validators;

public class AddProductValidator : AbstractValidator<AddProduct.AddProductCommand>
{
    public AddProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(p => p.Quantity)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(p => p.ImageUrl)
            .NotEmpty()
            .MaximumLength(800);

        RuleFor(p => p.Color)
            .NotEmpty()
            .IsInEnum();

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(p => p.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
