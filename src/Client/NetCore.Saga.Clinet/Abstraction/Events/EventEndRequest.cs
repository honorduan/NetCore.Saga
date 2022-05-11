using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public class EventEndRequest:EventRequest
    {
        public EventEndRequest(string globalTxId, string localTxId) : base(EventType.EventEnd, globalTxId, localTxId, "","", "", 0,"")
        {
        }
    }
}
