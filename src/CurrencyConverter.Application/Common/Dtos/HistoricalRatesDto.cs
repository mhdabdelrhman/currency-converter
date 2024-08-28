namespace CurrencyConverter.Common.Dtos;

public record HistoricalRatesDto
{
    public DateOnly Date { get; set; }

    public Dictionary<string, decimal> Rates { get; set; } = new();
}
