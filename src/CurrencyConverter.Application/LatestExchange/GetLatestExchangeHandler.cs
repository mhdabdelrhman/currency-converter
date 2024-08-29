using AutoMapper;
using CurrencyConverter.Common.Dtos;
using CurrencyConverter.Common.Helpers;
using CurrencyConverter.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.LatestExchange;

public class GetLatestExchangeHandler : IRequestHandler<GetLatestExchangeQuery, ExchangeRatesDto>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFrankfurterService _frankfurterService;
    private readonly IValidator<GetLatestExchangeQuery> _validator;

    public GetLatestExchangeHandler(ILogger<GetLatestExchangeHandler> logger
        , IMapper mapper
        , IFrankfurterService frankfurterService
        , IValidator<GetLatestExchangeQuery> validator
    )
    {
        _logger = logger;
        _mapper = mapper;
        _frankfurterService = frankfurterService;
        _validator = validator;
    }

    public async Task<ExchangeRatesDto> Handle(GetLatestExchangeQuery request, CancellationToken cancellationToken)
    {
        await ValidationHelper.ValidateAsync(_validator, request);

        var exchangeRates = await _frankfurterService.GetLatestExchangeAsync(request.Currency);

        return _mapper.Map<ExchangeRatesDto>(exchangeRates);
    }
}
