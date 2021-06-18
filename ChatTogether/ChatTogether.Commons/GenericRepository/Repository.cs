using ChatTogether.Commons.Pagination;
using ChatTogether.Commons.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
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

        public virtual async Task<PaginationPage<T>> GetManyAsync()
        {
            PaginationPage<T> page = await ctxt
                .Set<T>()
                .GetPaginationPageAsync();

            return page;
        }

        //TODO: sprawdzic pozniej na danych (np. jak beda wiadomosci w bazie)
        public virtual async Task<PaginationPage<T>> GetManyAsync(int page, int pageSize, Filter[] filters = null, Sorting[] sortings = null)
        {
            IQueryable<T> query = ctxt
                .Set<T>();

            if(filters.Length != 0)
            {
                foreach(Filter filter in filters)
                {
                    query.Where(string.Format("x => x.{0} {1} {2}", filter.FieldName, FilterOperations.operations[(int)filter.Operation], filter.Value));
                }
            }

            //https://dynamic-linq.net/basic-simple-query#ordering-results
            if (sortings.Length != 0)
            {
                StringBuilder sortingQuery = new StringBuilder(string.Empty);

                foreach(Sorting sorting in sortings)
                {
                    sortingQuery.Append(sorting.FieldName);
                    sortingQuery.Append(", ");
                    
                    if(!sorting.Ascending)
                    {
                        sortingQuery.Append("desc");
                    }
                }

                sortingQuery.Remove(sortingQuery.Length - 2, 2);

                query.OrderBy(sortingQuery.ToString());
            }

            PaginationPage<T> paginationPage = await query.GetPaginationPageAsync(page, pageSize);

            return paginationPage;
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
