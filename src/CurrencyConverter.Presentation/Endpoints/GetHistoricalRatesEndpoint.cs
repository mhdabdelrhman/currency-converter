using CurrencyConverter.Common.Dtos;
using CurrencyConverter.Historicals;
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
        Get("/api/historical-rates/{startDate}..{endDate}/{currency}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(GetHistoricalRatesRequest req, CancellationToken ct)
    {
        var query = new GetHistoricalRatesQuery(
            req.Currency,
            DateOnly.FromDateTime(req.StartDate),
            DateOnly.FromDateTime(req.EndDate),
            req.Skip,
            req.Limit
        );
        var page = await _mediator.Send(query, ct);

        await SendOkAsync(page, ct);
    }
}

public record GetHistoricalRatesRequest(string Currency, DateTime StartDate, DateTime EndDate, int? Skip = 0, int? Limit = AppConsts.DEFAULT_PAGE_SIZE);