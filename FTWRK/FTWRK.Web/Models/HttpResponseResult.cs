using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Models
{
    public class HttpResponseResult<T>
    {
        public int StatusCode { get; private set; }
        public T Data { get; private set; }
        public HttpResponseException Exception { get; private set; }

        private HttpResponseResult()
        {
        }

        public HttpResponseResult(int statusCode, T data, HttpResponseException exception = null)
        {
            StatusCode = statusCode;
            Data = data;
            Exception = exception;
        }

        public RedirectResult RedirectTo(string uri)
        {
            return new RedirectResult(uri);
        }
    }
}
