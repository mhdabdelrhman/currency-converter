using CurrencyConverter.Common.Dtos;

namespace CurrencyConverter.Historicals;

public record GetHistoricalRatesQuery(string Currency, DateOnly StartDate, DateOnly EndDate, int? Skip, int? Limit) : IRequest<HistoricalRatesPageDto>;
