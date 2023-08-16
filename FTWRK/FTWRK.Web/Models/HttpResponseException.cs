namespace FTWRK.Web.Models
{
    public class HttpResponseException
    {
        public string Message { get; set; }

        private HttpResponseException()
        {
        }

        public HttpResponseException(string message)
        {
            Message = message;
        }
    }
}
