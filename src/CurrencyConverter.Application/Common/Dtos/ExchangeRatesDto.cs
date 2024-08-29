namespace CurrencyConverter.Common.Dtos;

public record ExchangeRatesDto
{
    public decimal Amount { get; set; }

    public string Base { get; set; } = default!;

    public DateOnly Date { get; set; }

    public Dictionary<string, decimal> Rates { get; set; } = default!;
}
