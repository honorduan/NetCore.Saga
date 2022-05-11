using Microsoft.EntityFrameworkCore;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;
using Quartz;
using Quartz.Impl.Matchers;

namespace Kaytune.Saga.Server.Job
{
    [DisallowConcurrentExecution]
    public class LoadCompensationJob : JobAbstract<LoadCompensationJob>, IJob
    {
        public LoadCompensationJob(IBaseRepository<EventEntity> eventRepository,
            IBaseRepository<CompensationEntity> compensationRepository, ILogger<MinuteCompensationJob> logger) : base(
            eventRepository, compensationRepository, logger)
        {
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogDebug("job begin");

                //出异常的id
                var abortedIds =await _eventRepository.GetEvents().Where(c => c.Type == EventTypeEnum.Aborted)
                    .Select(c => c.GlobalId).Distinct().ToListAsync();
                
                var compensableEvents = await (from e in _eventRepository.GetEvents(c=>abortedIds.Contains(c.GlobalId))
                    join compensationEntity in _compensationRepository.GetEvents() on e.LocalId equals compensationEntity.LocalId 
                    into temp
                    from com in temp.DefaultIfEmpty()  
                    where e.Type==EventTypeEnum.EventEnd && e.CompensableMethod !=""  && com==null
                    select e).ToListAsync();
                
                foreach (var eventEntity in compensableEvents)
                {
                    await _compensationRepository.AddEventAsync(new CompensationEntity()
                    {
                        Type = EventTypeEnum.Aborted,
                        ServiceName = eventEntity.ServiceName,
                        CompensableMethod = eventEntity.CompensableMethod,
                        ServiceId = eventEntity.ServiceId,
                        GlobalId = eventEntity.GlobalId,
                        LocalId = eventEntity.LocalId,
                        Payloads = eventEntity.Payloads,
                        Retries = 0,
                        ImplementMethod = eventEntity.ImplementMethod,
                        Timestamp = eventEntity.Timestamp,
                        TypeName = eventEntity.TypeName,
                        ExceptionMessage = ""
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

