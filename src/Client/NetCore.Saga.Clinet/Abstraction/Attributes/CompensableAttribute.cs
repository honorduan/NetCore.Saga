using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Kaytune.Crm.Saga.Core.Transport.Impl;

namespace Kaytune.Crm.Saga.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Assembly)]
    public class CompensableAttribute : Attribute
    {
        public int Retries { get; set; }

        public string CompensationMethod { get; set; }

        public int RetryDelayInMilliseconds { get; set; }

        private readonly SagaContext _sagaContext;
        private readonly CompoensationContext _compensationContext;

        protected object InitInstance;

        protected MethodBase InitMethod;

        protected object[] Args;

        private readonly ISagaEventIntercept _sagaEventIntercept;
        private readonly IRecoveryPolicy _recoveryPolicy;
        private bool isException;

        /// <summary>
        /// CompensableAttribute
        /// </summary>
        /// <param name="compensationMethod"></param>
        public CompensableAttribute(string compensationMethod = "", int retryDelayInMilliseconds = 0, int timeout = 0, int retries = 0)
        {
            _sagaEventIntercept = ServiceLoader.Current.GetSrevice<ISagaEventIntercept>();
            _sagaContext = ServiceLoader.Current.GetSrevice<SagaContext>();
            _compensationContext = ServiceLoader.Current.GetSrevice<CompoensationContext>();
            _recoveryPolicy = ServiceLoader.Current.GetSrevice<IRecoveryPolicy>();
            Retries = retries;
            CompensationMethod = compensationMethod;
            RetryDelayInMilliseconds = retryDelayInMilliseconds;
        }
        public void Init(object instance, MethodBase method, object[] args)
        {
            InitInstance = instance;
            InitMethod = method;
            Args = args;
        }

        public void OnEntry()
        {
            isException = false;
            var type = InitInstance.GetType();
            Console.WriteLine("CompensableAttribute:OnEntry");
            _compensationContext.AddCompensationContext(type.GetMethod(CompensationMethod, BindingFlags.NonPublic | BindingFlags.Instance), type);
            _sagaContext.NewLocalTxId();
            _recoveryPolicy.BeforeApply(_sagaEventIntercept, _sagaContext, InitInstance.GetType().FullName, InitMethod.Name, CompensationMethod, Retries, Args);
        }

        public void OnExit()
        {

            var a = InitInstance;
            Console.WriteLine("CompensableAttribute:OnExit");
            _recoveryPolicy.AfterApply(_sagaEventIntercept);
        }

        public void OnException(Exception exception)
        {
            Console.WriteLine($"CompensableAttribute:exception");
            _recoveryPolicy.ErrorApply(_sagaEventIntercept, exception);
        }
        public void OnTaskContinuation(Task t)
        {
            t.Wait();
        }
    }
}
