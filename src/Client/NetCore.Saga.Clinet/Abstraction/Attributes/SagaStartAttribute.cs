using System.Reflection;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kaytune.Crm.Saga.Abstraction.Attributes
{

    /// <summary>
    /// SagaStartAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Module)]
    public class SagaStartAttribute:Attribute
    {
        private readonly SagaContext _sagaContext;

        protected object InitInstance;

        protected MethodBase InitMethod;

        protected object[] Args;
        private readonly ISagaStartEventIntercept _sagaEventIntercept;
        private bool isException;
        private ILogger<SagaStartAttribute> _logger => ServiceLoader.Current.GetSrevice<ILogger<SagaStartAttribute>>()??new NullLogger<SagaStartAttribute>();
        /// <summary>
        /// SagaStartAttribute
        /// </summary>
        /// <param name="sagaContext"></param>
        public SagaStartAttribute()
        {
            _sagaContext = ServiceLoader.Current.GetSrevice<SagaContext>();
            _sagaEventIntercept = ServiceLoader.Current.GetSrevice<ISagaStartEventIntercept>();
        }
        public void Init(object instance, MethodBase method, object[] args)
        {
            InitInstance = instance;
            InitMethod = method;
            Args = args;
        }

        public void OnEntry()
        {
            _logger.LogDebug("SagaStartAttribute OnEntry");
            isException = false;
            var globalId = _sagaContext.NewGlobalId();
            _sagaEventIntercept.PreIntercept(InitInstance.GetType().FullName,InitMethod.Name, "", 0, "",0);
        }

        public void OnExit()
        {
            _logger.LogDebug("SagaStartAttribute OnExit");
            if (!isException)
            {
                _sagaEventIntercept.Post();
                Console.WriteLine("OnExit");
            }
        }

        public void OnException(Exception exception)
        {
            _logger.LogDebug("SagaStartAttribute OnException");
            _sagaEventIntercept.Error(exception);
        }

        public void OnTaskContinuation(Task t)
        {
            t.Wait();
        }
    }
}
