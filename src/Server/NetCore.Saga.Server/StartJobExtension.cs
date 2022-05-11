using Kaytune.Saga.Server.Job;
using NetCore.Saga.Abstract;
using Quartz;
using Quartz.Impl;

namespace Kaytune.Saga.Server
{
    public static class StartJobExtension
    {
        public static async Task StartJob(IServiceProvider serviceProvider)
        {
            var load = JobBuilder.Create<LoadCompensationJob>()
                .Build();
            var loadTrigger = TriggerBuilder.Create()
                .WithCronSchedule("1/10 * * * * ?")
                .ForJob(load)
                .StartNow()
                .WithIdentity("load", "load").Build();

            var second = JobBuilder.Create<SecondCompensationJob>()
               .Build();
            var secondTrigger = TriggerBuilder.Create()
                .WithCronSchedule("10/10 * * * * ?")
                .ForJob(second)
                .StartNow()
                .WithIdentity("second", "second").Build();
         
            var minuteJob=JobBuilder.Create<MinuteCompensationJob>()
               .Build();
            var minuteTrigger = TriggerBuilder.Create()
                .WithCronSchedule("0 0/1 * * * ? ")
                .WithIdentity("minute", "minute")
                .StartNow()
                .ForJob(minuteJob)
                .Build();
            var schedulerFactory=ServiceLoader.Current.GetService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            scheduler.JobFactory = new JobFactory(serviceProvider);
            await scheduler.ScheduleJob(second, secondTrigger);
            await scheduler.ScheduleJob(minuteJob, minuteTrigger);
            await scheduler.ScheduleJob(load, loadTrigger);
            await scheduler.Start();
        }
    }
}
