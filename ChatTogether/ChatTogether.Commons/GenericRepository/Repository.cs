using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace ChatTogether.Commons.GenericRepository
{
    public abstract class Repository<T> : IRepository<T> where T : DboModel
    {
        protected readonly DbContext ctxt;

        public Repository(DbContext ctxt)
        {
            this.ctxt = ctxt;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await ctxt
                .Set<T>()
                .AddAsync(entity);

            await ctxt.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> exp)
        {
            T entity = await GetAsync(exp);

            ctxt
                .Set<T>()
                .Remove(entity);

            await ctxt.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> exp)
        {
            T entity = await ctxt
                .Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return entity;
        }

        public async Task<IEnumerable<T>> GetManyAsync()
        {
            //TODO: IEnumerable -> klasa z paginacji
            IEnumerable<T> entites = await ctxt
                .Set<T>()
                .ToListAsync();

            return entites;
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> exp)
        {
            //TODO: IEnumerable -> klasa z paginacji
            IEnumerable<T> entites = await ctxt
                .Set<T>()
                .Where(exp)
                .ToListAsync();

            return entites;
        }

        public async Task<T> UpdateAsync(Expression<Func<T, bool>> exp, T entity)
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
