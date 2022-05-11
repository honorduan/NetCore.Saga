using System.Net.NetworkInformation;
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
    /// ThreeCompensationJob
    /// </summary>
    [DisallowConcurrentExecution]
    public class SecondCompensationJob :JobAbstract<SecondCompensationJob>, IJob
    {
        public SecondCompensationJob(IBaseRepository<EventEntity> eventRepository, IBaseRepository<CompensationEntity> compensationRepository, ILogger<SecondCompensationJob> logger) : base(eventRepository, compensationRepository, logger)
        {
        }
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug("job is begin!");
            var compensationEntities = await _compensationRepository.GetEvents(c => c.Type == EventTypeEnum.Aborted && c.Retries<=3).ToListAsync();
            await base.SendMessage(compensationEntities);
        }
    }
}
