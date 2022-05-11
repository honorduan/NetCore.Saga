using System.Linq.Expressions;
using NetCore.Saga.Abstract.Entity;

namespace NetCore.Saga.Abstract.Repository
{
    public interface IBaseRepository<TSource>
    {
        Task<bool> AddEventAsync(TSource entity);
        Task<bool> UpdateEventAsync(int id, EventTypeEnum type,int retires,string exceptionMessage="");
        Task<bool> DeleteEventAsync(int id);
        IQueryable<TSource> GetEvents(Expression<Func<TSource, bool>> predicate=null);
        Task<TSource> GetFirstAsync(Expression<Func<TSource, bool>> predicate);
        Task<TSource> GetFirstOrDefaultAsync(Expression<Func<TSource, bool>> predicate );
    }
}
