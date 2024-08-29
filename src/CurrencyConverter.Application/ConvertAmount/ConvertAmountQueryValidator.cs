using FluentValidation;

namespace CurrencyConverter.ConvertAmount;

public class ConvertAmountQueryValidator : AbstractValidator<ConvertAmountQuery>
{
    public ConvertAmountQueryValidator()
    {
        RuleFor(x => x.FromCurrency).NotNull().Length(3);
        RuleFor(x => x.ToCurrency).NotNull().Length(3);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
