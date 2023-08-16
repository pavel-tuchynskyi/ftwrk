namespace FTWRK.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public string Message { get; private set; }

        private BadRequestException()
        {
        }

        public BadRequestException(string? message) : base(message)
        {
            Message = message;
        }
    }
}
