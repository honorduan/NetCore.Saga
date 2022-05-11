using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public interface IMessageSender
    {
        Task OnConnected();

        void OnDisconnected();

        void Close();

        EventResponse Send(EventRequest request);
    }
}
