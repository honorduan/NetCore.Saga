using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;

namespace Kaytune.Crm.Saga.Core.Transport.Impl
{
    public class DefaultRecoveryPolicy:IRecoveryPolicy
    {
       
        public void BeforeApply(ISagaEventIntercept sagaEventIntercept, SagaContext context, string typeName, string implementMethod,string compensationMethod, int retries ,params object[] parameters)
        {

            sagaEventIntercept.PreIntercept(typeName, implementMethod, compensationMethod, retries, parameters);
        }

        public void AfterApply(ISagaEventIntercept sagaEventIntercept)
        {
            sagaEventIntercept.Post();
        }

        public void ErrorApply(ISagaEventIntercept sagaEventIntercept, Exception throwable)
        {
            sagaEventIntercept.Error(throwable);
        }
    }
}
