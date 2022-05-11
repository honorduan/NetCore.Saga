using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public interface ISagaEventIntercept
    {
        /// <summary>
        /// PreIntercept
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="implementMethod"></param>
        /// <param name="compensationMethod"></param>
        /// <param name="retries"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        EventResponse PreIntercept(string typeName,string implementMethod, string compensationMethod,
            int retries, params object[] message);
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="implementMethod"></param>
        void Post();
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="implementMethod"></param>
        /// <param name="throwable"></param>
        void Error( System.Exception throwable);
    }
}
