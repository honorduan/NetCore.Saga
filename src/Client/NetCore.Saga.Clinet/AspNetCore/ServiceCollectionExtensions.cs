using Grpc.Net.Client;
using Kaytune.Crm.Core.Abstraction.Diagnostics;
using Kaytune.Crm.Core.Core.Diagnostics;
using Kaytune.Crm.Saga.Abstraction.Content;
using Kaytune.Crm.Saga.Abstraction.Events;
using Kaytune.Crm.Saga.Abstraction.ServiceExtend;
using Kaytune.Crm.Saga.Core.GracClient;
using Kaytune.Crm.Saga.Core.Transport.Impl;
using Kaytune.Crm.Saga.Transport.AspNetCore;
using Kaytune.Crm.Saga.Transport.HttpClient;
using Microsoft.Extensions.DependencyInjection;

namespace Kaytune.Crm.Saga.AspNetCore
{
    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// AddKaytuneSagaTest
        /// </summary>
        /// <param name="service"></param>
        public static SagaBuilder AddKaytuneSaga(this IServiceCollection services,Action<SagaOptions> optionAction)
        {
            SagaOptions options = new SagaOptions();
            var service = services.AddKaytuneSaga("");
            
            optionAction.Invoke(options);
            services.AddSingleton<IMessageSender>(new GrpcMessageClientSender(new GrpcServiceConfig()
            {
                ServiceId = options.ServiceId,
                ServiceName = options.ServiceName
            }, GrpcChannel.ForAddress(options.ServeUrl)));
            ServiceLoader.SetServiceLoader(services.BuildServiceProvider());
            return service;
        }

        public static SagaBuilder AddKaytuneSaga(this IServiceCollection services, string service)
        {
            SagaBuilder builder = new SagaBuilder(services);
            builder.Services.AddHostedService<DiagnosticHostService>();
            builder.Services.AddSingleton<DiagnosticListenerObserver>();
            builder.Services.AddSingleton<IDiagnosticIntercept, HostingDiagnosticIntercept>();
            builder.Services.AddSingleton<IDiagnosticIntercept, HttpClientDiagnosticIntercept>();
            builder.Services.AddSingleton<SagaContext>();
            builder.Services.AddSingleton<CompoensationContext>();
            builder.Services.AddSingleton<IMessageHandler, CompensableMessageHandler>();
            builder.Services.AddSingleton<IRecoveryPolicy, DefaultRecoveryPolicy>();
            builder.Services.AddSingleton<ISagaStartEventIntercept>(provider => new SagaStarAttributeProcessor(provider.GetService<SagaContext>(),provider.GetService<IMessageSender>()));
            builder.Services.AddSingleton<ISagaEventIntercept, CompensableInterceptor>();
            return builder;
        }
    }
}
