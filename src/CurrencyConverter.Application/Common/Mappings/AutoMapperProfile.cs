using AutoMapper;
using CurrencyConverter.Common.Dtos;
using CurrencyConverter.Common.Models;

namespace CurrencyConverter.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ExchangeRates, ExchangeRatesDto>();
    }
}
