using Domain.Exceptions;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce.API.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandelNotFoundEndPointAsync(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError($"something went error {ex}");

                await HandleExceptionAsync(httpContext, ex);

            }
        }

        private async Task HandelNotFoundEndPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $"End Point {httpContext.Request.Path} Not Found"
            }.ToString();
            await httpContext.Response.WriteAsync(response);
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //set default status code to 500
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // set content type => appliction\json
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError

            };
            //return status respons
            var response = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message
            }.ToString();

            await httpContext.Response.WriteAsync(response);
        }

    }
}
