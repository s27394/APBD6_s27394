using System.Text.RegularExpressions;
using APBD6.DTOs;
using FluentValidation;

namespace APBD6.validators;

public class AnimalReplaceRequestValidator : AbstractValidator<ReplaceAnimalRequest>
{
    public AnimalReplaceRequestValidator()
    {
        RuleFor(s => s.Name).MaximumLength(50).NotNull();
        RuleFor(s => s.Description).MaximumLength(50);
        RuleFor(s => s.Category).NotNull();
        RuleFor(s => s.Area).MaximumLength(50).NotNull();
    }
}