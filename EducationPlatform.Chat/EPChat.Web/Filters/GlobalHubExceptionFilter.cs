using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Filters
{
    internal class GlobalHubExceptionFilter : IHubFilter
    {
        private readonly ILogger<GlobalHubExceptionFilter> _logger;

        public GlobalHubExceptionFilter(ILogger<GlobalHubExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext context, 
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            try
            {
                return await next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception in hub method {MethodName}", context.HubMethodName);
                await context.Hub.Clients.Caller.SendAsync("ReceiveError", BuildProblemDetails(ex));
                return null;
            }
        }

        private static ProblemDetails BuildProblemDetails(Exception exception) => exception switch
        {
            KeyNotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not found",
                Detail = exception.Message
            },
            ValidationException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = exception.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Interanl server error",
                Detail = "An unexpected error occurred. Please try again later."
            }
        };
    }
}
