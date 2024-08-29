using CurrencyConverter.Common.Interfaces;
using CurrencyConverter.Common.Models;
using CurrencyConverter.Frankfurter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Refit;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ConverterOptions).Assembly);
        });

        services.AddOptions<ConverterOptions>()
            .BindConfiguration(nameof(ConverterOptions))
            .ValidateOnStart();

        services.AddOptions<FrankfurterOptions>()
            .BindConfiguration(nameof(FrankfurterOptions))
            .ValidateOnStart();

        services.AddRefitClient<IFrankfurterAPI>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var frankfurterOptions = sp.GetRequiredService<IOptions<FrankfurterOptions>>().Value;

                httpClient.BaseAddress = new Uri(frankfurterOptions.ApiBaseUrl);
            });

        services.AddScoped<IFrankfurterService, FrankfurterService>();

        return services;
    }
}
