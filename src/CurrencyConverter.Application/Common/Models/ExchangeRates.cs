namespace CurrencyConverter.Common.Models;

public class ExchangeRates
{
    public decimal Amount { get; set; }

    public string Base { get; set; } = default!;

    public DateOnly Date { get; set; }

    public Dictionary<string, decimal> Rates { get; set; } = default!;
}
