using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Kaytune.Crm.Saga.AspNetCore
{
    public class SagaOptions
    {
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public string ServeUrl { get; set; }

        public SagaOptions()
        {
            
        }
    }
}
