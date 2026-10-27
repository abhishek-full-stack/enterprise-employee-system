using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var title = "Internal Server Error";
            var detail = "An unexpected error occurred.";
            var type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

            if (exception is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                title = "Bad Request";
                detail = exception.Message;
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            }

            var problem = new ProblemDetails
            {
                Type = type,
                Title = title,
                Status = statusCode,
                Detail = detail,
                Instance = context.Request?.Path.Value
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(problem, options);
            return context.Response.WriteAsync(result);
        }
    }
}
