using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Commons.GenericRepository
{
    public abstract class Repository<Tkey, T> : IRepository<Tkey, T> where T : DboModel<Tkey>
    {
        protected readonly DbContext ctxt;

        public Repository(DbContext ctxt)
        {
            this.ctxt = ctxt;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await ctxt
                .Set<T>()
                .AddAsync(entity);

            await ctxt.SaveChangesAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(Expression<Func<T, bool>> exp)
        {
            T entity = await GetAsync(exp);

            ctxt
                .Set<T>()
                .Remove(entity);

            await ctxt.SaveChangesAsync();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> exp)
        {
            T entity = await ctxt
                .Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync()
        {
            IEnumerable<T> entites = await ctxt
                .Set<T>()
                .AsNoTracking()
                .ToListAsync();

            return entites;
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> exp)
        {
            IEnumerable<T> entites = await ctxt
                .Set<T>()
                .Where(exp)
                .AsNoTracking()
                .ToListAsync();

            return entites;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            ctxt
                .Set<T>()
                .Update(entity);

            await ctxt.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(Expression<Func<T, bool>> exp, T entity)
        {
            T currentEntity = await GetAsync(exp);

            entity.Id = currentEntity.Id;

            ctxt
                .Set<T>()
                .Update(entity);

            await ctxt.SaveChangesAsync();

            return entity;
        }
    }
}
