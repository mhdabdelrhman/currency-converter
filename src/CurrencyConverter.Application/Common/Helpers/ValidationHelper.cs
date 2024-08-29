using CurrencyConverter.Common.Exceptions;
using FluentValidation;

namespace CurrencyConverter.Common.Helpers;

internal static class ValidationHelper
{
    public static async Task ValidateAsync<T>(IValidator<T> validator, T obj)
    {
        var validationResult = await validator.ValidateAsync(obj);
        if (validationResult.IsValid)
            return;

        throw new ApiValidationException(validationResult.Errors);
    }
}
