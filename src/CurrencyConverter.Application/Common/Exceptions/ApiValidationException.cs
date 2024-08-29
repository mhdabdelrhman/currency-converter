using FluentValidation;
using FluentValidation.Results;

namespace CurrencyConverter.Common.Exceptions;

public class ApiValidationException : ValidationException
{
    public ApiValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
    {
    }
}
