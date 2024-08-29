using FluentValidation;

namespace CurrencyConverter.Historicals;

public class GetHistoricalRatesQueryValidator : AbstractValidator<GetHistoricalRatesQuery>
{
    public GetHistoricalRatesQueryValidator()
    {
        RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.Currency).NotNull().Length(3);
    }
}
