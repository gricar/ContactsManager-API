using Contacts37.Application.Common.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Contacts37.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                //NotFoundException => HttpStatusCode.NotFound,
                //UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError,
            };

            var response = new
            {
                statusCode = (int)statusCode,
                error = ex.Message,
                details = ex is BadRequestException badRequestException
                    ? badRequestException.ErrorDetails
                    : null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
