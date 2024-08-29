using CurrencyConverter.Common.Dtos;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.LatestExchange;

public class GetLatestExchangeHandler : IRequestHandler<GetLatestExchangeQuery, ExchangeRatesDto>
{
    private readonly ILogger _logger;

    public GetLatestExchangeHandler(ILogger<GetLatestExchangeHandler> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRatesDto> Handle(GetLatestExchangeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
