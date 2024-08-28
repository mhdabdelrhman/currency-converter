using CurrencyConverter.Common.Models;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ConverterOptions).Assembly);
        });

        services.Configure<ConverterOptions>(options =>
        {
            options.ConvertAmountNotSupportedCurrencies = ["TRY", "PLN", "THB", "MXN"];
        });

        return services;
    }
}
