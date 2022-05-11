using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCore.Saga.Abstract.Entity;

namespace NetCore.Saga.Abstract.Repository
{
    public class BaseRepository<T>:IBaseRepository<T> where T:BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<bool> AddEventAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEventAsync(int id, EventTypeEnum type, int retires, string exceptionMessage = "")
        {
            var entity = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (entity != null)
            {
                entity.Type = type;
                entity.Retries = retires;
                entity.ExceptionMessage = exceptionMessage;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public IQueryable<T> GetEvents(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet;
            }
            return _dbSet.Where(predicate).AsNoTracking();
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new Exception("predicate is null ");
            }
            return await _dbSet.AsNoTracking().FirstAsync(predicate);
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new Exception("predicate is null ");
            }
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
    }
}
