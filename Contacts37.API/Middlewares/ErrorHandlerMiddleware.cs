using Contacts37.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace Contacts37.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        /*public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature == null) return;

                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.ContentType = "application/json";

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        BadRequestException => (int)HttpStatusCode.BadRequest,
                        OperationCanceledException => (int)HttpStatusCode.ServiceUnavailable,
                        //NoDataFoundException => (int)HttpStatusCode.NotFound,
                        _ => (int)HttpStatusCode.InternalServerError
                    };

                    var errorResponse = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = contextFeature.Error.GetBaseException().Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                });
            });
        }*/
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
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
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
