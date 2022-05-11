using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Net.Client;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Kaytune.Saga.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kaytune.Crm.Saga.Core.GracClient
{
    public class GrpcMessageClientSender : IMessageSender
    {
        private readonly SagaEnvetService.SagaEnvetServiceClient _envetServiceClient;
        private IMessageHandler _messageHandler => ServiceLoader.Current.GetSrevice<IMessageHandler>();
        private readonly GrpcServiceConfig _serviceConfig;
        private ILogger<GrpcMessageClientSender> _logger => ServiceLoader.Current.GetSrevice<ILogger<GrpcMessageClientSender>>() ?? new NullLogger<GrpcMessageClientSender>();
        public GrpcMessageClientSender(GrpcServiceConfig serviceConfig, GrpcChannel channel)
        {
            _envetServiceClient = new SagaEnvetService.SagaEnvetServiceClient(channel);
            _serviceConfig = serviceConfig;
        }
        public async Task OnConnected()
        {

            _logger.LogDebug($"{_serviceConfig.ServiceName}_{_serviceConfig.ServiceId} is OnConnected");
            // _messageHandler = ServiceLoader.Current.GetSrevice<IMessageHandler>();
            var connect = _envetServiceClient.OnConnect(new SagaConfig()
            {
                ServiceId = _serviceConfig.ServiceId,
                ServiceName = _serviceConfig.ServiceName
            });

            while (await connect.ResponseStream.MoveNext(CancellationToken.None))
            {
                _logger.LogDebug($"response :{JsonSerializer.Serialize(connect.ResponseStream.Current)}");
                var message = connect.ResponseStream.Current;
                _messageHandler.OnReceive(message.GlobalId, message.LocalId, message.TypeName, message.CompensationMethod, message.Payloads.ToByteArray());
            }
        }

        public void OnDisconnected()
        {
            _envetServiceClient.OnDisconnected(new SagaConfig()
            { ServiceId = _serviceConfig.ServiceId, ServiceName = _serviceConfig.ServiceName });
        }

        public void Close()
        {
            _logger.LogDebug($"{_serviceConfig.ServiceName}_{_serviceConfig.ServiceId} is Close");
        }

        public EventResponse Send(EventRequest request)
        {
            _logger.LogDebug($"GrpcMessageClientSender {JsonSerializer.Serialize(request)}");
            if (string.IsNullOrEmpty(request.GlobalId))
            {
                return new EventResponse(true);
            }
            var ack = _envetServiceClient.SendEvent(new GrpcEventMessage()
            {
                GlobalId = request.GlobalId,
                Type = request.Type.ToString(),
                ServiceId = _serviceConfig.ServiceId,
                ServiceName = _serviceConfig.ServiceName,
                CompensationMethod = request.CompensationMethod,
                Payloads = ByteString.CopyFrom(JsonSerializer.SerializeToUtf8Bytes(new object[] { "1121313123131" })),
                Retries = request.Retries,
                LocalId = request.LocalId ?? "",
                ImplementMethod = request.ImplementMethod,
                TypeName = request.TypeName,
                Timestamp = request.Timestamp,
            });
            return new EventResponse(ack.Aborted);
        }
    }
}
