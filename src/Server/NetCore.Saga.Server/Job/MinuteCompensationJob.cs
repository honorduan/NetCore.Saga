using Google.Protobuf;
using Grpc.Core;
using Kaytune.Saga.Server.Services;
using Microsoft.EntityFrameworkCore;
using NetCore.Saga.Abstract;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;
using Quartz;

namespace Kaytune.Saga.Server.Job
{
    /// <summary>
    /// MinuteCompensationJob
    /// </summary>
    [DisallowConcurrentExecution]
    public class MinuteCompensationJob :JobAbstract<MinuteCompensationJob>, IJob
    {
        public MinuteCompensationJob(IBaseRepository<EventEntity> eventRepository, IBaseRepository<CompensationEntity> compensationRepository, ILogger<MinuteCompensationJob> logger) : base(eventRepository, compensationRepository, logger)
        {
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug("job begin");
            var compensationEntities = await _compensationRepository.GetEvents(c => c.Type == EventTypeEnum.Aborted && c.Retries > 3).ToListAsync();
            await base.SendMessage(compensationEntities);
        }

      
    }
}
