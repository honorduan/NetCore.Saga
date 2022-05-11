using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCore.Saga.Abstract.Entity;

namespace NetCore.Saga.Server.Mysql.Context
{
    public class SagaDbContext:DbContext
    {
        public SagaDbContext(DbContextOptions<SagaDbContext> options) : base(options)
        {

        }
        public DbSet<EventEntity> Event { get; set; }
        public DbSet<CompensationEntity> Compensation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
