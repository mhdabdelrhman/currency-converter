using FastEndpoints;
using FastEndpoints.Swagger;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurePresentationServices(configuration)
            .ConfigureInfrastructureServices(configuration)
        ;

        return services;
    }

    private static IServiceCollection ConfigurePresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();

        // add fast end points
        services.AddFastEndpoints()
            .SwaggerDocument(o =>
            {
                o.AutoTagPathSegmentIndex = 2;
                o.ShortSchemaNames = true;
                o.TagDescriptions = t =>
                {
                    t["Converter"] = "Currency Converter APIs";
                };
                o.TagCase = TagCase.TitleCase;
                o.EnableJWTBearerAuth = false;
                o.ExcludeNonFastEndpoints = true;
                //o.RemoveEmptyRequestSchema = true;                
                o.DocumentSettings = s =>
                {
                    s.Title = "Currency Converter API";
                    s.Version = "v1";
                };
            });

        return services;
    }
}
