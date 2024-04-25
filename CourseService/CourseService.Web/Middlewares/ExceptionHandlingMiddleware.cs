using CourseService.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;

namespace CourseService.Web.Middlewares {
    public class ExceptionHandlingMiddleware : IMiddleware {
        private readonly ILogger _logger; 
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try {
                await next(context);
            }
            catch (Exception e) {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = 500;
                ProblemDetails details = new() {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = e.Message,
                    //Errors = GetErrors(exception)
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(details));
            }
        }

        private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception) {
            IReadOnlyDictionary<string, string[]> errors = null;
            if (exception is ValidationException validationException) {
                errors = validationException.ErrorsDictionary;
            }
            return errors;
        }
    }
}
