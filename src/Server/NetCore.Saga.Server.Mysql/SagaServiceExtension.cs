using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;
using NetCore.Saga.Server.Mysql.Context;
using NetCore.Saga.Server.Mysql.Repository;

namespace NetCore.Saga.Server.Mysql
{
    /// <summary>
    /// SagaServiceExtension
    /// </summary>
    public static class SagaServiceExtension
    {
        public static IServiceCollection UseMySql(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddDbContext<SagaDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("saga"),MySqlServerVersion.LatestSupportedServerVersion);
               
            });
            services.AddScoped<IBaseRepository<EventEntity>,EventRepository>();
            services.AddScoped<IBaseRepository<CompensationEntity>,CompensationRepository>();
            return services;
        }
    }
}
