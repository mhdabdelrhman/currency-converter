using CurrencyConverter.Common.Exceptions;
using System.Net;

namespace CurrencyConverter.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger
            , RequestDelegate next
        )
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        #region Private Methods
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = GetStatusCode(exception, context);
            var message = GetErrorMessage(exception);
            var details = GetErrorDetails(exception);

            await SendErrorAsync(context, statusCode, message, details);
        }

        private async Task SendErrorAsync(HttpContext context, int statusCode, string message, string details)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                Status = "error",
                Message = message,
                Details = details,
                StatusCode = statusCode,
            });
        }

        private int GetStatusCode(Exception exception, HttpContext context)
        {
            if (exception is ApiException apiException)
                return apiException.StatusCode;

            if (exception is ApiValidationException)
                return (int)HttpStatusCode.BadRequest;

            return (int)HttpStatusCode.InternalServerError;
        }

        private string GetErrorMessage(Exception exception)
        {
            if (exception is ApiException)
                return "API Error.";

            if (exception is ApiValidationException)
                return "Validation Error.";

            return "Server Error.";
        }

        private string GetErrorDetails(Exception exception)
        {
            return exception.Message;
        }
        #endregion
    }
}
