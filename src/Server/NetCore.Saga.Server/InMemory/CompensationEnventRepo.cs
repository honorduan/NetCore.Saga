using System.Linq.Expressions;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;

namespace Kaytune.Saga.Server.InMemory
{
    public class CompensationEnventRepo: IBaseRepository<CompensationEntity>
    {
        private static readonly List<CompensationEntity> Repo = new List<CompensationEntity>();
        public Task<bool> AddEventAsync(CompensationEntity entity)
        {
            var compensation = Repo.FirstOrDefault(c => c.Id == entity.Id);
            if (compensation == null)
            {
                entity.Id= Repo.Any() ? Repo.Max(c => c.Id) + 1 : 1;
                Repo.Add(entity);
            }

            return Task.FromResult(true);
        }

        public Task<bool> UpdateEventAsync(int id, EventTypeEnum type,int retries,string exceptionMessage)
        {
            var compensation = Repo.FirstOrDefault(c => c.Id == id);
            if (compensation != null)
            {
                compensation.Retries = retries;
                compensation.Type = type;
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeleteEventAsync(int id)
        {

            Repo.RemoveAll(c => c.Id == id);
            return Task.FromResult(true);
        }

        public IQueryable<CompensationEntity> GetEvents(Expression<Func<CompensationEntity, bool>> predicate)
        {
            if (predicate ==null)
            {
                return Repo.AsQueryable();
            }
            return Repo.AsQueryable().Where(predicate);
        }

        public Task<CompensationEntity> GetFirstAsync(Expression<Func<CompensationEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new Exception("predicate is null");
            }
            return Task.FromResult(Repo.First(predicate.Compile()));
        }

        public Task<CompensationEntity> GetFirstOrDefaultAsync(Expression<Func<CompensationEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new Exception("predicate is null");
            }
            return Task.FromResult(Repo.FirstOrDefault(predicate.Compile()));
        }
    }
}
