using CurrencyConverter.Common.Exceptions;
using CurrencyConverter.Common.Interfaces;
using CurrencyConverter.Common.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace CurrencyConverter.Frankfurter;

public class FrankfurterService(ILogger<FrankfurterService> logger, IFrankfurterAPI frankfurterAPI)
    : IFrankfurterService
{
    const int MAX_RETRY_COUNT = 3;

    public async Task<ExchangeRates> GetLatestExchangeAsync(string currency)
    {
        var exchangeRates = await GetLatestExchangeAndConvertAsync(currency);

        return exchangeRates!;
    }

    public async Task<ExchangeRates> ConvertAmountAsync(string fromCurrency, string? toCurrency, decimal? amount)
    {
        var exchangeRates = await GetLatestExchangeAndConvertAsync(fromCurrency, toCurrency, amount);

        return exchangeRates!;
    }

    public async Task<HistoricalRates> GetHistoricalRatesAsync(DateOnly startDate, DateOnly? endDate, string? fromCurrency, string? toCurrency, decimal? amount)
    {
        var retryPolicy = BuildRetryPolicy<Exception>();
        var response = await retryPolicy.ExecuteAsync(async () =>
        {
            return await frankfurterAPI.GetHistoricalsAsync(
                startDateString: startDate.ToString("yyyy-MM-dd"),
                endDateString: endDate?.ToString("yyyy-MM-dd"),
                from: fromCurrency,
                to: toCurrency,
                amount: amount
            );
        });

        await CheckApiResponseAsync(response);

        var historicalRates = await response.Content.ReadFromJsonAsync<HistoricalRates>();

        return historicalRates!;
    }

    #region Private Methods
    private async Task<ExchangeRates> GetLatestExchangeAndConvertAsync(string baseCurrency, string? toCurrency = null, decimal? amount = null)
    {
        var retryPolicy = BuildRetryPolicy<Exception>();
        var response = await retryPolicy.ExecuteAsync(async () =>
        {
            return await frankfurterAPI.GetLatestAsync(
                from: baseCurrency,
                to: toCurrency,
                amount: amount
            );
        });

        await CheckApiResponseAsync(response);

        var exchangeRates = await response.Content.ReadFromJsonAsync<ExchangeRates>();

        return exchangeRates!;
    }

    private async Task CheckApiResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var apiResponseStatusCode = (int)response.StatusCode;
        var apiResponseObject = await response.Content.ReadFromJsonAsync<JsonObject>();
        var apiResponseMessage = apiResponseObject?["message"]?.ToString()
            ?? "invalid parameters";

        throw new ApiException(apiResponseStatusCode, apiResponseMessage);
    }

    private AsyncRetryPolicy BuildRetryPolicy<TException>() where TException : Exception
    {
        return Policy.Handle<TException>()
            .WaitAndRetryAsync(MAX_RETRY_COUNT,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, sleepTime, context) =>
                {
                    logger.LogWarning($"Retry call Frankfurter API, sleep time: {sleepTime.TotalSeconds}s, exception: {exception?.Message}.");
                }
            );
    }
    #endregion
}
