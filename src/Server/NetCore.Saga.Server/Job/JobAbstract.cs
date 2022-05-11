using Google.Protobuf;
using Grpc.Core;
using Kaytune.Saga.Server.Services;
using NetCore.Saga.Abstract;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;
using Quartz;
using Quartz.Logging;

namespace Kaytune.Saga.Server.Job
{
    public abstract class JobAbstract<T> where T : class
    {
        
        protected readonly IBaseRepository<EventEntity> _eventRepository;
        protected readonly IBaseRepository<CompensationEntity> _compensationRepository;
        protected readonly ILogger _logger;

        public JobAbstract(IBaseRepository<EventEntity> eventRepository, IBaseRepository<CompensationEntity> compensationRepository, ILogger logger)
        {
            _eventRepository = eventRepository;
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public abstract Task Execute(IJobExecutionContext context);

        protected async Task SendMessage(List<CompensationEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var compensationEntity in entities)
            {
                try
                {
                    IServerStreamWriter<GrpcCompensableArgs> responseStream;
                    if (!SagaEventService.ResponseStream.TryGetValue(
                            $"{compensationEntity.ServiceName}_{compensationEntity.ServiceId}", out responseStream))
                    {
                        var key = SagaEventService.ResponseStream.Keys.FirstOrDefault(c =>
                            c.Contains($"{compensationEntity.ServiceName}"));
                        if (key != null)
                        {
                            responseStream = SagaEventService.ResponseStream[key];
                        }
                    }

                    if (responseStream != null)
                    {
                        await _compensationRepository.UpdateEventAsync(compensationEntity.Id, compensationEntity.Type,
                            compensationEntity.Retries + 1);
                        await responseStream.WriteAsync(new GrpcCompensableArgs()
                        {
                            CompensableMethod = compensationEntity.CompensableMethod,
                            Payloads = ByteString.CopyFrom(compensationEntity.Payloads),
                            GlabolId = compensationEntity.GlobalId,
                            LocalId = compensationEntity.LocalId,
                            ImplementMethod = compensationEntity.ImplementMethod,
                            TypeName = compensationEntity.TypeName
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
