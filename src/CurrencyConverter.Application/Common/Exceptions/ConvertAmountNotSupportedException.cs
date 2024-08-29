namespace CurrencyConverter.Common.Exceptions
{
    public class ConvertAmountNotSupportedException : SystemException
    {
        public ConvertAmountNotSupportedException(string? message) : base(message)
        {
        }
    }
}
