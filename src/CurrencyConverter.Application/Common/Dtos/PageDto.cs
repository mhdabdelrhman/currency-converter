namespace CurrencyConverter.Common.Dtos;

public record PageDto<T>
{
    public long Total { get; set; }

    public IEnumerable<T>? Items { get; set; }
}
