using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaytune.Crm.Saga.Abstraction.Content;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    /// <summary>
    /// IRecoveryPolicy
    /// </summary>
    public interface IRecoveryPolicy
    {
        void BeforeApply(ISagaEventIntercept sagaEventIntercept, SagaContext context, string typeName,string implementName ,string compensationMethod, int retries, params object[] parameters);
        void AfterApply(ISagaEventIntercept sagaEventIntercept);
        void ErrorApply(ISagaEventIntercept sagaEventIntercept, Exception throwab);
    }
}
