using CurrencyConverter.Middlewares;
using FastEndpoints;
using FastEndpoints.Swagger;
using HealthChecks.UI.Client;
using System.Net;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseMiddlewares(this WebApplication app, IConfiguration configuration)
    {
        app.UsePresentationMiddlewares(configuration);

        return app;
    }

    private static WebApplication UsePresentationMiddlewares(this WebApplication app, IConfiguration configuration)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        app.UseOutputCache();

        app.UseHttpsRedirection();

        app.UseFastEndpoints(c =>
        {
            c.Endpoints.ShortNames = true;
        });

        app.UseSwaggerGen();
        app.UseSwaggerUi();

        app.MapHealthChecks("/_health", new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        }).CacheOutput(x => x.NoCache()); // Disable default caching for healthcheck        

        app.MapRootPage();

        return app;
    }

    private static WebApplication MapRootPage(this WebApplication app)
    {
        app.Map("/", async (HttpContext context) =>
        {
            var html = """
                <h1> Currency Converter V1</h1>                
                <a href="/swagger"> Swagger UI </a>
                """;

            context.Response.StatusCode = (int)HttpStatusCode.OK;            
            await context.Response.WriteAsync(html);
        });

        return app;
    }
}
