namespace CurrencyConverter.Common.Dtos;

public record HistoricalRatesPageDto
{
    public decimal Amount { get; set; }

    public string Base { get; set; } = default!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }    

    public DateOnly? PageStartDate { get; set; }

    public DateOnly? PageEndDate { get; set; }

    public Dictionary<DateOnly, Dictionary<string, decimal>> PageRates { get; set; } = new();

}
