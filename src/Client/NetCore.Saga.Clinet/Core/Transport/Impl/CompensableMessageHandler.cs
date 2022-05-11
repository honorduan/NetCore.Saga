using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Kaytune.Saga.Client;

namespace Kaytune.Crm.Saga.Core.Transport.Impl
{
    public class CompensableMessageHandler:IMessageHandler
    {
        private readonly IMessageSender _sender;
        private readonly CompoensationContext _context;
        public CompensableMessageHandler(IMessageSender sender, CompoensationContext context)
        {
            _sender = sender;
            _context = context;
        }
        public void OnReceive(string globalId, string localId, string typeName,string compensableMethod,
            params byte[] payloads)
        {
            try
            {
                _context.Apply(typeName, compensableMethod, payloads);
                _sender.Send(new EventCompensatedRequest(globalId, localId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
