using FluentValidation;

namespace CurrencyConverter.LatestExchange;

public class GetLatestExchangeQueryValidator : AbstractValidator<GetLatestExchangeQuery>
{
    public GetLatestExchangeQueryValidator()
    {
        RuleFor(x => x.Currency).NotNull().Length(3);
    }
}
