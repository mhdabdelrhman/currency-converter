using System.Runtime.Serialization;

namespace CurrencyConverter.Common.Exceptions
{
    public class ConvertAmountNotSupportedException : SystemException
    {
        public ConvertAmountNotSupportedException()
        {
        }

        public ConvertAmountNotSupportedException(string? message) : base(message)
        {
        }

        public ConvertAmountNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConvertAmountNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
