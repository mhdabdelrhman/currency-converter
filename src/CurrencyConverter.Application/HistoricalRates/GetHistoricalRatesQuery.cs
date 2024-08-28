using CurrencyConverter.Common.Dtos;

namespace CurrencyConverter.HistoricalRates;

public record GetHistoricalRatesQuery(string Currency,
    DateOnly? Start,
    DateOnly? End,
    int? Skip,
    int? Limit
) : IRequest<HistoricalRatesPageDto>;
