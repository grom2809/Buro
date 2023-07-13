namespace Bankrupt.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public int StatusCode { get; set; }
        public ValidationException(string message, int code)
        : base(message)
        {
            StatusCode = code;
        }
    }
}
