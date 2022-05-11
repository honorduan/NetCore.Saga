using Kaytune.Crm.Core.Abstraction.Diagnostics;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Kaytune.Crm.Saga.Transport.AspNetCore
{
    public class HostingDiagnosticIntercept:IDiagnosticIntercept
    {
        public string ListenerName { get; } = "Microsoft.AspNetCore";
        private readonly SagaContext _sagaContext;
        public HostingDiagnosticIntercept()
        {
            _sagaContext = ServiceLoader.Current.GetSrevice<SagaContext>();
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.BeginRequest")]
        public void BenginRequest(HttpContext httpContext)
        {
            string globalTxId= httpContext.Request.Headers[SagaContext.GlobalTxIdKey];
            if (globalTxId==null)
            {
                Console.WriteLine($"globalTxId is null");
            }
            else
            {
                _sagaContext.SetGlobalId(globalTxId);
                _sagaContext.SetLocalTxId(httpContext.Request.Headers[SagaContext.GlobalTxIdKey]);
            }
        }
    }
}
