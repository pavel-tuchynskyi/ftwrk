using FTWRK.Application.Common.Exceptions;
using FTWRK.Web.Models;
using MediatR;

namespace FTWRK.Web.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var httpEx = new HttpResponseException(exception.Message);

            await httpContext.Response.WriteAsJsonAsync(new HttpResponseResult<Unit>(httpContext.Response.StatusCode, Unit.Value, exception: httpEx));
        }
    }
}
