using CurrencyConverter.Common.Dtos;
using CurrencyConverter.HistoricalRates;
using FastEndpoints;
using MediatR;

namespace CurrencyConverter.Endpoints;

public class GetHistoricalRatesEndpoint : Endpoint<GetHistoricalRatesRequest, HistoricalRatesPageDto>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public GetHistoricalRatesEndpoint(ILogger<GetHistoricalRatesEndpoint> logger
        , IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/api/historical-rates/{currency}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(GetHistoricalRatesRequest req, CancellationToken ct)
    {
        var query = new GetHistoricalRatesQuery(req.Currency, req.Start, req.End, req.Skip, req.Limit);
        var page = await _mediator.Send(query, ct);

        await SendOkAsync(page, ct);
    }
}

public record GetHistoricalRatesRequest(string Currency,
    DateOnly? Start,
    DateOnly? End,
    int? Skip,
    int? Limit
);