using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public class EventAboutRequest:EventRequest
    {
        /// <summary>
        /// The payloads max length.
        /// </summary>
        private const int PayloadsMaxLength = 500;
        public EventAboutRequest(string globalTxId,string localTxId,
           Exception throwable) : base(EventType.Aborted, globalTxId, localTxId, "", "", "", 0, StackTrace(throwable))
        {
        }
        private static string StackTrace(System.Exception throwable)
        {
            if (throwable.Message.Length > PayloadsMaxLength)
            {
                return throwable.Message.Substring(PayloadsMaxLength);
            }

            return throwable.Message;
        }
    }
}
