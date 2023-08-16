namespace FTWRK.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public string Message { get; private set; }

        private ValidationException()
        {
        }

        public ValidationException(string? message) : base("Invalid preperties values")
        {
            Message = message;
        }
    }
}
