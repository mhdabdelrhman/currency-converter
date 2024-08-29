namespace CurrencyConverter.Common.Exceptions
{
    public class ApiConvertNotSupportedException : SystemException
    {
        public ApiConvertNotSupportedException(string? message) : base(message)
        {
        }
    }
}
