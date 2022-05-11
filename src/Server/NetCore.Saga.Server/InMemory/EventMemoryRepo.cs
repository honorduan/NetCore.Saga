using System.Linq.Expressions;
using Microsoft.Extensions.Primitives;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;

namespace Kaytune.Saga.Server.InMemory
{
    /// <summary>
    /// EventMemoryRepo
    /// </summary>
    public class EventMemoryRepo : IBaseRepository<EventEntity>
    {
        private static readonly List<EventEntity> Repo = new List<EventEntity>();

        /// <summary>
        /// AddEventAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> AddEventAsync(EventEntity entity)
        {
            var eventEntity =Repo.FirstOrDefault(c => c.Id == entity.Id);
            if (eventEntity == null)
            {
                entity.Id = Repo.Any()? Repo.Max(c => c.Id) + 1:1;
                Repo.Add(entity);
            }
            return Task.FromResult(true);
        }

        public Task<bool> UpdateEventAsync(int id,EventTypeEnum type,int retires,string exceptionMessage)
        {
            var entity = Repo.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                entity.Type = type;
                entity.ExceptionMessage = exceptionMessage;
            }
            return Task.FromResult(true);
        }

        public Task<bool> DeleteEventAsync(int id)
        {
            Repo.RemoveAll(c => c.Id == id);
            return Task.FromResult(true);
        }

        /// <summary>
        /// GetEventsAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<EventEntity> GetEvents(Expression<Func<EventEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return Repo.AsQueryable();
            }

            return Repo.AsQueryable().Where(predicate);
        }

        /// <summary>
        /// GetFirstAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<EventEntity> GetFirstAsync(Expression<Func<EventEntity, bool>> predicate)
        {
            if (predicate==null)
            {
                throw new Exception("predicate is null");
            }
            return Task.FromResult(Repo.First(predicate.Compile()));
        }

        /// <summary>
        /// GetFirstOrDefaultAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<EventEntity> GetFirstOrDefaultAsync(Expression<Func<EventEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new Exception("predicate is null");
            }
            return Task.FromResult(Repo.FirstOrDefault(predicate.Compile()));
        }
    }
}
