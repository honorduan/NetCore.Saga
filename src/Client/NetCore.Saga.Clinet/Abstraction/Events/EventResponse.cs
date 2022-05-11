using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public class EventResponse
    {
        public bool Aborted { get; set; }
        public EventResponse(bool aborted)
        {
            Aborted = aborted;
        }
    }
}
