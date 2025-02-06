using FluentValidation;

namespace ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .WithMessage("Id is required");
        RuleFor(c => c.Name)
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters");
    }
}