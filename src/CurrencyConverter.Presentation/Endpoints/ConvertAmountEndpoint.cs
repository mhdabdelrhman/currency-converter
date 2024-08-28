using CurrencyConverter.Common.Exceptions;
using CurrencyConverter.ConvertAmount;
using FastEndpoints;
using MediatR;
using System.Net;

namespace CurrencyConverter.Endpoints;

public class ConvertAmountEndpoint : Endpoint<ConvertAmountRequest>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ConvertAmountEndpoint(ILogger<ConvertAmountEndpoint> logger
        , IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/api/convert-amount/{from}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConvertAmountRequest req, CancellationToken ct)
    {
        var query = new ConvertAmountQuery(req.From, req.To, req.Amount);
        try
        {
            var currencyConvert = await _mediator.Send(query, ct);

            await SendOkAsync(currencyConvert, ct);
        }
        catch (ConvertAmountNotSupportedException)
        {
            await SendAsync("Bad request", statusCode: (int)HttpStatusCode.BadRequest, cancellation: ct);
        }
    }

}

public record ConvertAmountRequest(string From, string? To, decimal? Amount);
