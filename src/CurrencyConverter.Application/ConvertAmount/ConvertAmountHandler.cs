using CurrencyConverter.Common.Dtos;
using CurrencyConverter.Common.Exceptions;
using CurrencyConverter.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyConverter.ConvertAmount;

public class ConvertAmountHandler : IRequestHandler<ConvertAmountQuery, CurrencyConvertDto>
{
    private readonly ILogger _logger;
    private readonly ConverterOptions _converterOptions;

    public ConvertAmountHandler(ILogger<ConvertAmountHandler> logger
        , IOptions<ConverterOptions> converterOptions
    )
    {
        _logger = logger;
        _converterOptions = converterOptions.Value;
    }

    public Task<CurrencyConvertDto> Handle(ConvertAmountQuery request, CancellationToken cancellationToken)
    {
        if (!IsConvertAmountSupported(request.FromCurrency))
            throw new ConvertAmountNotSupportedException($"Convert amount for {request.FromCurrency} is not supported.");

        throw new NotImplementedException();
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
