using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.Core.Transport.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class CompensableInterceptor : ISagaEventIntercept
    {
        private readonly IMessageSender _messageSender;
        private readonly SagaContext _sagaContext;
        public CompensableInterceptor(SagaContext context, IMessageSender sender)
        {
            _sagaContext = context;
            _messageSender=sender;
        }
        public EventResponse PreIntercept(string typeName,string implementMethod, string compensationMethod, int retries,
            params object[] message)
        {
            return _messageSender.Send(new EventStartRequest(_sagaContext.GetGlobalId(), _sagaContext.GetLocalTxId(),
                typeName,implementMethod, compensationMethod, retries,message));
        }

        public void Post()
        {
            _messageSender.Send(new EventEndRequest(_sagaContext.GetGlobalId(), _sagaContext.GetLocalTxId()));
        }

        public void Error(Exception throwable)
        {
            _messageSender.Send(new EventAboutRequest(_sagaContext.GetGlobalId(), _sagaContext.GetLocalTxId(),throwable));
        }
    }
}
