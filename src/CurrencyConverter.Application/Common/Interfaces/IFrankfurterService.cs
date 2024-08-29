using CurrencyConverter.Common.Models;

namespace CurrencyConverter.Common.Interfaces;

public interface IFrankfurterService
{
    Task<ExchangeRates> GetLatestExchangeAsync(string currency);

    Task<ExchangeRates> ConvertAmountAsync(string fromCurrency, string? toCurrency = null, decimal? amount = null);

    Task<HistoricalRates> GetHistoricalRatesAsync(DateOnly startDate, DateOnly? endDate = null, string? fromCurrency = null, string? toCurrency = null, decimal? amount = null);
}
