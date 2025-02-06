using FluentValidation;

namespace ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required")
            .NotNull().WithMessage("Category name is required")
            .MaximumLength(50).WithMessage("Category name must be less than 50 characters");
        
        RuleFor(c => c.IconName)
            .Must(x => x is not null)
            .When(c => c.ParentId is null)
            .WithMessage("Icon name is required")
            .Must(x => x is null)
            .When(c => c.ParentId is not null)
            .WithMessage("Child category can't have icon name");
    }
}