namespace CurrencyConverter.Common.Exceptions;

public class ApiException : SystemException
{
    public int StatusCode { get; }

    public ApiException(int statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }
}
