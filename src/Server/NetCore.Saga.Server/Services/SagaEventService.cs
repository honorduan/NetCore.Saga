using System.Collections.Concurrent;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using NetCore.Saga.Abstract;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;

namespace Kaytune.Saga.Server.Services
{
    /// <summary>
    /// SagaEventService
    /// </summary>
    public class SagaEventService:SagaEnvetService.SagaEnvetServiceBase
    {
        
        public static Dictionary<string, IServerStreamWriter<GrpcCompensableArgs>> ResponseStream =
            new Dictionary<string, IServerStreamWriter<GrpcCompensableArgs>>();

        private readonly IBaseRepository<EventEntity> _eventRepository;
        private readonly IBaseRepository<CompensationEntity> _compensationRepository;
        
        public SagaEventService(IBaseRepository<EventEntity> eventRepository, IBaseRepository<CompensationEntity> compensationRepository)
        {
            _eventRepository= eventRepository;
            _compensationRepository= compensationRepository;
        }

        /// <summary>
        /// OnConnect
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task OnConnect(SagaConfig request, IServerStreamWriter<GrpcCompensableArgs> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"{request.ServiceName}:{request.ServiceId} is Connect");
            ResponseStream.TryAdd($"{request.ServiceName}_{request.ServiceId}",responseStream);
            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10),context.CancellationToken);
                }
            }
            catch
            {
                Console.WriteLine($"{request.ServiceName}_{request.ServiceId} is cancel");
                ResponseStream.Remove($"{request.ServiceName}_{request.ServiceId}");
            }
        }
        /// <summary>
        /// SendEvent
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GrpcAck> SendEvent(GrpcEventMessage request, ServerCallContext context)
        {
            var compensationEntity = await _compensationRepository.GetFirstOrDefaultAsync(c => c.GlobalId == request.GlabolId && c.LocalId == request.LocalId);
            if (compensationEntity != null)
            {
                var eventEntity = await _eventRepository.GetFirstAsync(c=>c.GlobalId == request.GlabolId && c.LocalId==request.LocalId);
                await _eventRepository.UpdateEventAsync(eventEntity.Id, Enum.Parse<EventTypeEnum>(request.Type, true),0);
                await _compensationRepository.UpdateEventAsync(compensationEntity.Id,
                        Enum.Parse<EventTypeEnum>(request.Type, true), compensationEntity.Retries);
                return new GrpcAck() { Aborted = false };
            }

            var message = await _eventRepository.GetFirstOrDefaultAsync(c =>
                 c.ServiceName == request.ServiceName && c.GlobalId == request.GlabolId &&
                 c.LocalId == request.LocalId);
            if (message == null)
            {
                await _eventRepository.AddEventAsync(new EventEntity()
                {
                    TypeName = request.TypeName,
                    GlobalId = request.GlabolId,
                    LocalId = request.LocalId,
                    Payloads = request.Payloads.ToByteArray(),
                    Retries = request.Retries,
                    ServiceId = request.ServiceId,
                    ServiceName = request.ServiceName,
                    Timestamp = request.Timestamp,
                    Type = Enum.Parse<EventTypeEnum>(request.Type),
                    CompensableMethod = request.CompensableMethod,
                    ImplementMethod = request.ImplementMethod,
                    ExceptionMessage = request.ExceptionMessage
                });
            }
            else
            {
                await _eventRepository.UpdateEventAsync(message.Id, Enum.Parse<EventTypeEnum>(request.Type), request.Retries,request.ExceptionMessage);
            }
            if (Enum.Parse<EventTypeEnum>(request.Type)==EventTypeEnum.Aborted)
            {
                var eventEntities =await _eventRepository.GetEvents(c =>
                    c.GlobalId == request.GlabolId && c.LocalId != "" &&
                    c.Type == EventTypeEnum.EventEnd).ToListAsync();
                var compensationEntities= eventEntities.Select(c => new CompensationEntity()
                    {
                        GlobalId = c.GlobalId,
                        LocalId = c.LocalId,
                        ServiceName = c.ServiceName,
                        Type =EventTypeEnum.Aborted,
                        CompensableMethod = c.CompensableMethod,
                        Retries = 1,
                        Payloads = c.Payloads,
                        ImplementMethod = c.ImplementMethod,
                        ServiceId = c.ServiceId,
                        Timestamp = c.Timestamp,
                        TypeName = c.TypeName,
                        ExceptionMessage = ""
                    }).ToList();
                foreach (var entity in compensationEntities)
                {
                    await _compensationRepository.AddEventAsync(entity);
                    if (ResponseStream.TryGetValue($"{entity.ServiceName}_{entity.ServiceId}", out IServerStreamWriter<GrpcCompensableArgs> responseStream))
                    {
                        await responseStream.WriteAsync(new GrpcCompensableArgs()
                        {
                            Payloads = ByteString.CopyFrom(entity.Payloads),
                            CompensableMethod = entity.CompensableMethod,
                            GlabolId = entity.GlobalId,
                            LocalId = entity.LocalId,
                        });
                    }
                }
            }
            return new GrpcAck() { Aborted = false };
        }

        /// <summary>
        /// 主动断开连接
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<GrpcAck> OnDisconnected(SagaConfig request, ServerCallContext context)
        {
            if (ResponseStream.TryGetValue($"{request.ServiceName}_{request.ServiceId}", out _))
            {
                ResponseStream.Remove($"{request.ServiceName}_{request.ServiceId}");
            }

            return Task.FromResult(new GrpcAck() { Aborted = false });
        }
    }
}
