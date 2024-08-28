using CurrencyConverter.LatestExchange;
using FastEndpoints;
using MediatR;

namespace CurrencyConverter.Endpoints;

public class GetLatestExchangeEndpoint : Endpoint<GetLatestExchangeRequest>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public GetLatestExchangeEndpoint(ILogger<GetLatestExchangeEndpoint> logger
        , IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/api/latest-exchange/{currency}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLatestExchangeRequest req, CancellationToken ct)
    {
        var query = new GetLatestExchangeQuery(req.Currency);
        var currencyExchangeDto = await _mediator.Send(query, ct);

        await SendOkAsync(currencyExchangeDto, ct);
    }
}

public record GetLatestExchangeRequest(string Currency);
