using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kaytune.Crm.Saga.Abstraction.ServiceExtend
{
    public class ServiceLoader
    {
        private readonly IServiceProvider _serviceCurrentProvider;
        private static IServiceProvider _serviceProvider;
        public ServiceLoader(IServiceProvider serviceProvider)
        {
            _serviceCurrentProvider = serviceProvider;
        }

        public static ServiceLoader Current => new ServiceLoader(_serviceProvider);

        public static void SetServiceLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public TService GetSrevice<TService>()
        {
            return _serviceCurrentProvider.GetService<TService>();
        }
    }
}
