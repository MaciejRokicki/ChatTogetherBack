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

        public virtual async Task<PaginationPage<T>> GetManyAsync()
        {
            PaginationPage<T> page = await ctxt
                .Set<T>()
                .AsNoTracking()
                .GetPaginationPageAsync();

            return page;
        }

        //TODO: sprawdzic pozniej na danych (np. jak beda wiadomosci w bazie)
        public virtual async Task<PaginationPage<T>> GetPageAsync(PaginationQuery paginationQuery)
        {
            IQueryable<T> query = ctxt
                .Set<T>();

            if (paginationQuery.Filters.Length != 0)
            {
                foreach (Filter filter in paginationQuery.Filters)
                {
                    query.Where(string.Format("x => x.{0} {1} {2}", filter.FieldName, FilterOperations.operations[(int)filter.Operation], filter.Value));
                }
            }

            //https://dynamic-linq.net/basic-simple-query#ordering-results
            if (paginationQuery.Sortings.Length != 0)
            {
                StringBuilder sortingQuery = new StringBuilder(string.Empty);

                foreach (Sorting sorting in paginationQuery.Sortings)
                {
                    sortingQuery.Append(sorting.FieldName);
                    sortingQuery.Append(", ");

                    if (!sorting.Ascending)
                    {
                        sortingQuery.Append("desc");
                    }
                }

                sortingQuery.Remove(sortingQuery.Length - 2, 2);

                query
                    .OrderBy(sortingQuery.ToString())
                    .AsNoTracking();
            }

            PaginationPage<T> paginationPage = await query.GetPaginationPageAsync(paginationQuery.Page, paginationQuery.PageSize);

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
