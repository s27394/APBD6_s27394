using APBD6.DTOs;
using FluentValidation;

namespace APBD6.validators;

public class AnimalCreateRequestValidator : AbstractValidator<CreateAnimalRequest>
{
    public AnimalCreateRequestValidator()
    {
        RuleFor(s => s.Name).MaximumLength(50).NotNull();
        RuleFor(s => s.Description).MaximumLength(50);
        RuleFor(s => s.Category).NotNull();
        RuleFor(s => s.Area).MaximumLength(50).NotNull();
    }

}