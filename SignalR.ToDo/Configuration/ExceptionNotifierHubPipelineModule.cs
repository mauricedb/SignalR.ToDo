using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR.ToDo.Configuration
{
    /// <summary>
    /// From: http://stackoverflow.com/questions/12608874/signalr-exception-logging
    /// </summary>
    public class ExceptionNotifierHubPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(Exception ex, IHubIncomingInvokerContext context)
        {
            var aggregateException = ex as AggregateException;
            if (aggregateException != null)
            {
                NotifyClientAggregateExceptionHandler(aggregateException, context);
            }
            else
            {
                NotifyClientExcectionHandler(ex, context);
            }
        }

        private static void NotifyClientAggregateExceptionHandler(AggregateException ex, IHubIncomingInvokerContext context)
        {
            ex.Handle(e =>
            {
                NotifyClientExcectionHandler(e, context);
                return true;
            });
        }

        private static void NotifyClientExcectionHandler(Exception ex, IHubIncomingInvokerContext context)
        {
            var baseException = ex.GetBaseException();
            var message = baseException.Message;
            context.Hub.Clients.Caller.ExceptionHandler(message);
        }
    }
}