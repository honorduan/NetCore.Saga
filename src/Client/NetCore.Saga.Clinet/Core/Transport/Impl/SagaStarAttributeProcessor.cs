using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.Core.Transport.Impl
{
    /// <summary>
    /// SagaStarAttributeProcessor
    /// </summary>
    public class SagaStarAttributeProcessor:ISagaStartEventIntercept
    {
        private readonly SagaContext _context;
        private readonly IMessageSender _messageSender;
        private readonly ILogger<SagaStarAttributeProcessor> _logger;
        public SagaStarAttributeProcessor(SagaContext sagaContext,IMessageSender messageSender)
        {
            _context = sagaContext;
            _messageSender = messageSender;
            _logger = ServiceLoader.Current.GetSrevice<ILogger<SagaStarAttributeProcessor>>()??new NullLogger<SagaStarAttributeProcessor>();
        }
        public EventResponse PreIntercept(string typeName,string compensationMethod, string retryMethod, int retries,
            params object[] message)
        {
            _logger.LogDebug("PreIntercept");
            return _messageSender.Send(new EventStartRequest(_context.GetGlobalId(), _context.GetLocalTxId(),
                typeName, compensationMethod, retryMethod,0, message));
        }

        public void Post()
        {
            _logger.LogDebug("Post");
            var response = _messageSender.Send(new EventEndRequest(_context.GetGlobalId(), _context.GetLocalTxId()));
            if (response.Aborted)
            {
                throw new Exception($"transaction {_context.GetGlobalId()}  is aborted");
            }
        }

        public void Error(Exception throwable)
        {
            _logger.LogDebug("Error");
            _messageSender.Send(new EventAboutRequest(_context.GetGlobalId(),_context.GetLocalTxId(),throwable));
        }
    }
}
