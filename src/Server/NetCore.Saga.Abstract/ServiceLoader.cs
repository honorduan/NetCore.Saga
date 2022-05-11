using Microsoft.Extensions.DependencyInjection;

namespace NetCore.Saga.Abstract
{
    public class ServiceLoader
    {
        private readonly IServiceProvider _serviceCurrentProvider;

        private static IServiceProvider _serviceProvider;
        public static ServiceLoader Current => new ServiceLoader(_serviceProvider) ;
        public ServiceLoader(IServiceProvider serviceProvider)

        {
            _serviceCurrentProvider = serviceProvider;
        }

        public static void SetServiceLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider=serviceProvider;
        }

        public T GetService<T>()
        {
            return _serviceCurrentProvider.GetService<T>();
        }
    }
}
