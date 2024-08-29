using AutoMapper;
using CurrencyConverter.Common.Dtos;
using CurrencyConverter.Common.Exceptions;
using CurrencyConverter.Common.Helpers;
using CurrencyConverter.Common.Interfaces;
using CurrencyConverter.Common.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyConverter.ConvertAmount;

public class ConvertAmountHandler : IRequestHandler<ConvertAmountQuery, ExchangeRatesDto>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ConverterOptions _converterOptions;
    private readonly IFrankfurterService _frankfurterService;
    private readonly IValidator<ConvertAmountQuery> _validator;

    public ConvertAmountHandler(ILogger<ConvertAmountHandler> logger
        , IMapper mapper
        , IOptions<ConverterOptions> converterOptions
        , IFrankfurterService frankfurterService
        , IValidator<ConvertAmountQuery> validator
    )
    {
        _logger = logger;
        _mapper = mapper;
        _converterOptions = converterOptions.Value;
        _frankfurterService = frankfurterService;
        _validator = validator;
    }

    public async Task<ExchangeRatesDto> Handle(ConvertAmountQuery request, CancellationToken cancellationToken)
    {
        await ValidationHelper.ValidateAsync(_validator, request);

        if (!IsConvertAmountSupported(request.FromCurrency))
            throw new ConvertAmountNotSupportedException($"Convert amount from {request.FromCurrency} is not supported.");

        if (!IsConvertAmountSupported(request.ToCurrency))
            throw new ConvertAmountNotSupportedException($"Convert amount to {request.ToCurrency} is not supported.");

        var exchangeRates = await _frankfurterService.ConvertAmountAsync(request.FromCurrency, request.ToCurrency, request.Amount);

        return _mapper.Map<ExchangeRatesDto>(exchangeRates);
    }

    #region Private Methods
    private bool IsConvertAmountSupported(string currency)
    {
        if (_converterOptions.ConvertAmountNotSupportedCurrencies is not null)
        {
            if (_converterOptions.ConvertAmountNotSupportedCurrencies.Any(
                x => x.Equals(currency, StringComparison.CurrentCultureIgnoreCase)
            ))
            {
                return false;
            }
        }

        return true;
    }
    #endregion
}
