using Kaytune.Crm.Core.Abstraction.Diagnostics;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Kaytune.Crm.Saga.Transport.HttpClient
{
    /// <summary>
    /// HttpClientDiagnosticIntercept
    /// </summary>
    public class HttpClientDiagnosticIntercept:IDiagnosticIntercept
    {
        public string ListenerName { get; } = "HttpHandlerDiagnosticListener";
        private readonly SagaContext _sagaContext;
        /// <summary>
        /// 
        /// </summary>
        public HttpClientDiagnosticIntercept()
        {
            _sagaContext = ServiceLoader.Current.GetSrevice<SagaContext>();
        }

        [DiagnosticName("System.Net.Http.Request")]
        public void BeginRequest(HttpRequestMessage request)
        {
            if (_sagaContext?.GetGlobalId() == null) return;
            request.Headers.Add(SagaContext.GlobalTxIdKey, _sagaContext.GetGlobalId());
            request.Headers.Add(SagaContext.LocalTxIdKey, _sagaContext.GetLocalTxId());
        }
    }
}
