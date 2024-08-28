using CurrencyConverter.Common.Dtos;

namespace CurrencyConverter.ConvertAmount;

public record ConvertAmountQuery(string FromCurrency,
    string? ToCurrency,
    decimal? Amount
) : IRequest<CurrencyConvertDto>;
