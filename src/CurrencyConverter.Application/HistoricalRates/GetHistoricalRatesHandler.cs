using CurrencyConverter.Common.Dtos;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.HistoricalRates;

public class GetHistoricalRatesHandler : IRequestHandler<GetHistoricalRatesQuery, HistoricalRatesPageDto>
{
    private readonly ILogger _logger;

    public GetHistoricalRatesHandler(ILogger<GetHistoricalRatesHandler> logger)
    {
        _logger = logger;
    }

    public Task<HistoricalRatesPageDto> Handle(GetHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
