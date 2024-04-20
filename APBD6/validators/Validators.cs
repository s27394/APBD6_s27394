using FluentValidation;

namespace APBD6.validators;

public static class Validators
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AnimalReplaceRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AnimalCreateRequestValidator>();
    }
}