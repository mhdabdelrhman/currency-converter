using Refit;

namespace CurrencyConverter.Frankfurter;

public interface IFrankfurterAPI
{
    [Get("/latest")]
    Task<HttpResponseMessage> GetLatestAsync(string? from, string? to, decimal? amount);


    [Get("/{startDateString}..{endDateString}")]
    Task<HttpResponseMessage> GetHistoricalsAsync(string startDateString, string? endDateString, string? from, string? to, decimal? amount);
}
