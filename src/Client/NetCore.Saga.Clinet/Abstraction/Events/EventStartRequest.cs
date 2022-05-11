using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public class EventStartRequest:EventRequest
    {
        public EventStartRequest(string globalTxId, string localTxId, string typeName,string implementMethod,  string compensationMethod, int retries,  params object[] payloads) : base(EventType.EventStart, globalTxId, localTxId, typeName, implementMethod, compensationMethod,0, "", payloads)
        {
        }
    }
}
