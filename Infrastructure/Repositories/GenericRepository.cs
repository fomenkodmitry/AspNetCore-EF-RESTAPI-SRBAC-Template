using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Base;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DbContext _context;

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public IQueryable Get<TEntity>() where TEntity : BaseModel
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> Get<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseModel
        {
            return _context.Set<TEntity>().AsNoTracking().Where(predicate).AsQueryable();
        }

        public TEntity GetOne<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseModel
        {
            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetById<TEntity>(Guid id) where TEntity : BaseModel
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Create<TEntity>(TEntity item) where TEntity : BaseModel
        {
            item.DateCreated = DateTime.UtcNow;
            item.IsActive ??= true;
            item.DateUpdated = DateTime.UtcNow;
            await _context.Set<TEntity>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task Update<TEntity>(TEntity item) where TEntity : BaseModel
        {
            item.DateUpdated = DateTime.UtcNow;
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Remove<TEntity>(TEntity item) where TEntity : BaseModel
        {
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetWithInclude<TEntity>(
            params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : BaseModel
        {
            return Include(includeProperties);
        }

        public IQueryable<TEntity> GetWithInclude<TEntity>(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : BaseModel
        {
            var query = Include(includeProperties);
            return query.Where(predicate).AsQueryable();
        }

        private IQueryable<TEntity> Include<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : BaseModel
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}