using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    /// <summary>
    /// EventType
    /// </summary>
    public enum EventType
    {
        EventStart,
        EventEnd,
        Aborted,
        Compensated,
    }
}
