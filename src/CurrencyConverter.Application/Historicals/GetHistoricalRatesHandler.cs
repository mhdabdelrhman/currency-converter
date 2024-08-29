using CurrencyConverter.Common.Dtos;
using CurrencyConverter.Common.Helpers;
using CurrencyConverter.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Historicals;

public class GetHistoricalRatesHandler : IRequestHandler<GetHistoricalRatesQuery, HistoricalRatesPageDto>
{
    const int DEFAULT_PAGE_SIZE = 10;

    private readonly ILogger _logger;
    private readonly IFrankfurterService _frankfurterService;
    private readonly IValidator<GetHistoricalRatesQuery> _validator;

    public GetHistoricalRatesHandler(ILogger<GetHistoricalRatesHandler> logger
        , IFrankfurterService frankfurterService
        , IValidator<GetHistoricalRatesQuery> validator
    )
    {
        _logger = logger;
        _frankfurterService = frankfurterService;
        _validator = validator;
    }

    public async Task<HistoricalRatesPageDto> Handle(GetHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        await ValidationHelper.ValidateAsync(_validator, request);

        var skip = request.Skip ?? 0;
        var limit = request.Limit ?? DEFAULT_PAGE_SIZE;

        var historicalRatesPage = BuildPageData(1, request);

        var (pageStartDate, pageEndDate) = CalculatePageStartAndEndDates(request.StartDate, request.EndDate, skip, limit);
        if (pageStartDate > request.EndDate)
            return historicalRatesPage;

        if (pageEndDate > request.EndDate)
            pageEndDate = request.EndDate;

        historicalRatesPage.PageStartDate = pageStartDate;
        historicalRatesPage.PageEndDate = pageEndDate;

        var historicalRates = await _frankfurterService.GetHistoricalRatesAsync(pageStartDate, pageEndDate, request.Currency);

        historicalRatesPage.PageRates = historicalRates.Rates;

        return historicalRatesPage;
    }

    #region Private Method
    private static HistoricalRatesPageDto BuildPageData(decimal amount, GetHistoricalRatesQuery request)
    {
        return new HistoricalRatesPageDto
        {
            Amount = amount,
            Base = request.Currency.ToUpper(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };
    }

    private (DateOnly pageStartDate, DateOnly pageEndDate) CalculatePageStartAndEndDates(DateOnly startDate, DateOnly endDate, int skip, int limit)
    {
        var pageStartDate = startDate.AddDays(skip);
        var pageEndDate = pageStartDate.AddDays(limit);

        return (pageStartDate, pageEndDate);
    }
    #endregion
}
