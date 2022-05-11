using System.Diagnostics;
using Grpc.Core;
using Kaytune.Crm.Core.Core.Diagnostics;
using Kaytune.Crm.Saga.Abstraction.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kaytune.Crm.Saga.AspNetCore
{
    public class DiagnosticHostService:IHostedService
    {
        private readonly DiagnosticListenerObserver _diagnosticListenerObserver;
        private readonly IMessageSender _messageSender;
        private readonly ILogger _logger;
        public DiagnosticHostService(DiagnosticListenerObserver diagnosticListenerObserver, IMessageSender messageSender, ILogger<DiagnosticHostService> logger)
        {
            this._diagnosticListenerObserver = diagnosticListenerObserver;
            _messageSender = messageSender;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var task= new TaskFactory().StartNew(async () =>
            {
                DiagnosticListener.AllListeners.Subscribe(_diagnosticListenerObserver);
                bool isConnected=false;
                while (!isConnected)
                {
                    try
                    {
                        await _messageSender.OnConnected();
                        isConnected = true;
                    }
                    catch (RpcException e)
                    {
                        _logger.LogError($"grpc is connected {e.Message},retrying");
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
               
            }, cancellationToken);
            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageSender.OnDisconnected();
           return Task.CompletedTask;
        }
    }
}
