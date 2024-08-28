using CurrencyConverter.Common.Dtos;

namespace CurrencyConverter.LatestExchange;

public record GetLatestExchangeQuery(string Currency) : IRequest<CurrencyExchangeDto>;
