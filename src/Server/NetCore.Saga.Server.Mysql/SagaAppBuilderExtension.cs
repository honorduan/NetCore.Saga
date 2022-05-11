using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Saga.Abstract;
using NetCore.Saga.Server.Mysql.Context;

namespace NetCore.Saga.Server.Mysql
{
    public  static class SagaAppBuilderExtension
    {
        public static void InitDb(this IApplicationBuilder builder)
        {
            
            var context = ServiceLoader.Current.GetService<SagaDbContext>();
            if (context != null)
            {
                context.Database.Migrate();
            }
            
        }
    }
}
