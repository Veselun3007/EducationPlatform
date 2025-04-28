using Chat.Web.Models;
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

        public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext context, Func<HubInvocationContext, ValueTask<object?>> next)
        {
            try
            {
                return await next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception in hub method {MethodName}", context.HubMethodName);

                await context.Hub.Clients.Caller.SendAsync("ReceiveError", ReturnError(ex));
                return null;
            }
        }

        private static Error ReturnError(Exception ex)
        {
            return ex switch
            {
                ValidationException => Errors.ValidationFailed(),
                KeyNotFoundException => Errors.NotFound(),
                _ => Errors.Unpredictable()
            };
        }
    }
}
