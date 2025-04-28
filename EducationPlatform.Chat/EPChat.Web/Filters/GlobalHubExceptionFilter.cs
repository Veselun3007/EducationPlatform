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
                var hub = context.Hub;
                var sendMethod = "ReceiveError";
                var errorWrapper = MessageWrapper<string>.Error(ReturnError(ex));
                await hub.Clients.Caller.SendAsync(sendMethod, errorWrapper);
                return null;
            }
        }

        private static Error ReturnError(Exception ex)
        {
            return ex switch
            {
                ValidationException => Errors.ValidationFailed(ex.Message),
                KeyNotFoundException => Errors.NotFound(ex.Message),
                _ => Errors.Unpredictable()
            };
        }
    }
}
