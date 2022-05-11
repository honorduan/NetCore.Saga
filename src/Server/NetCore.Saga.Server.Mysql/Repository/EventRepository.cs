using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;
using NetCore.Saga.Server.Mysql.Context;

namespace NetCore.Saga.Server.Mysql.Repository
{
    public class EventRepository:BaseRepository<EventEntity>,IBaseRepository<EventEntity>
    {
        public EventRepository(SagaDbContext dbContext):base(dbContext)
        {
        }
    }
}
