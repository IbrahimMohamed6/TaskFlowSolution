using System.Net;
using System.Text.Json;
using TaskFlow.Application.Common;

namespace TaskFlow.API.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, response) = exception switch
            {
                ValidationException ve => ((int)HttpStatusCode.BadRequest,
                    ApiResponse<object>.Fail(ve.Message, ve.Errors)),

                NotFoundException => ((int)HttpStatusCode.NotFound,
                    ApiResponse<object>.Fail(exception.Message)),

                UnauthorizedException => ((int)HttpStatusCode.Unauthorized,
                    ApiResponse<object>.Fail(exception.Message)),

                _ => ((int)HttpStatusCode.InternalServerError,
                    ApiResponse<object>.Fail(_environment.IsDevelopment() ? exception.ToString() : "An unexpected error occurred."))
            };

            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
