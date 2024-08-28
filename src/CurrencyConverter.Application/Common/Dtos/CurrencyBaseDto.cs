namespace CurrencyConverter.Common.Dtos;

public abstract record CurrencyBaseDto
{
    public decimal Amount { get; set; } = 1;

    public string Base { get; set; } = default!;

    public DateOnly Date { get; set; }

    public Dictionary<string, decimal> Rates { get; set; } = new();
}
