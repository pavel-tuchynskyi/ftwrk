namespace FTWRK.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Message { get; private set; }

        private NotFoundException()
        {
        }

        public NotFoundException(string? message) : base(message)
        {
            Message = message;
        }
    }
}
