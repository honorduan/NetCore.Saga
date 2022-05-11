using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public interface IMessageHandler
    {
        void OnReceive(string globalId, string localId, string typeName, string compensableMethod, params byte[] payloads);
    }
}
