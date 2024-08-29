using CurrencyConverter.ConvertAmount;
using FastEndpoints;
using MediatR;

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
        Get("/api/convert-amount/{amount}/{from}/{to}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConvertAmountRequest req, CancellationToken ct)
    {
        var query = new ConvertAmountQuery(req.From, req.To, req.Amount);
        var exchangeRates = await _mediator.Send(query, ct);

        await SendOkAsync(exchangeRates, ct);
    }

}

public record ConvertAmountRequest(string From, string To, decimal Amount);
