using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.Abstraction.Events
{
    public class EventRequest
    {
        public long Timestamp { get; set; }
        public EventType Type { get; set; }
        public string GlobalId { get; set; }
        public string LocalId { get; set; }
        public string TypeName { get; set; }
        public string ImplementMethod { get; set; }
        public object[] Payloads { get; set; }

        public string CompensationMethod { get; set; }
        public int Retries { get; set; }
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// EventRequest
        /// </summary>
        /// <param name="type"></param>
        /// <param name="globalId"></param>
        /// <param name="localId"></param>
        /// <param name="typeName"></param>
        /// <param name="implementMethod"></param>
        /// <param name="compensationMethod"></param>
        /// <param name="retries"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="payloads"></param>
        public EventRequest(EventType type,
            string globalId,
            string localId,
            string typeName,
            string implementMethod,
            string compensationMethod,
            int retries,
            string exceptionMessage,
            params object[] payloads)
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Type = type;
            GlobalId = globalId;
            LocalId = localId;
            TypeName = typeName;
            ImplementMethod = implementMethod;
            CompensationMethod = compensationMethod;
            Retries = retries;
            Payloads = payloads;
            ExceptionMessage = exceptionMessage??"";
        }
    }
}
