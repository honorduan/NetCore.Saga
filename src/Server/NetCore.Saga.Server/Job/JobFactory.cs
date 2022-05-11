using Quartz;
using Quartz.Spi;

namespace Kaytune.Saga.Server.Job
{
    public class JobFactory:IJobFactory
    {
        private  readonly  IServiceProvider _serviceProvider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope=_serviceProvider.CreateScope();
            
            return (IJob)scope.ServiceProvider.GetService(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
