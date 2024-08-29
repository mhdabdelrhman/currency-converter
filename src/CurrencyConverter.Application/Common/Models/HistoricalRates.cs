namespace CurrencyConverter.Common.Models;

public class HistoricalRates
{
    public decimal Amount { get; set; }

    public string Base { get; set; } = default!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Dictionary<DateOnly, Dictionary<string, decimal>> Rates { get; set; } = default!;
}
