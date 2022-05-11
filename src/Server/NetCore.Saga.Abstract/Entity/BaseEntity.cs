using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Saga.Abstract.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public EventTypeEnum Type { get; set; }
        public long Timestamp { get; set; }
        public string GlobalId { get; set; }
        public string LocalId { get; set; }
        public string TypeName { get; set; }
        public string ImplementMethod { get; set; }
        public byte[] Payloads { get; set; }
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public string CompensableMethod { get; set; }
        public int Retries { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
