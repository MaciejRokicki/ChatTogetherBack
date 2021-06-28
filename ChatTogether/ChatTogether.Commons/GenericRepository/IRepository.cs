using ChatTogether.Commons.Pagination.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Commons.GenericRepository
{
    public interface IRepository<Tkey, T> where T : DboModel<Tkey>
    {
        Task<T> GetAsync(Expression<Func<T, bool>> exp);
        Task<PaginationPage<T>> GetManyAsync();
        Task<PaginationPage<T>> GetPageAsync(PaginationQuery paginationQuery);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> UpdateAsync(Expression<Func<T, bool>> exp, T entity);
        Task DeleteAsync(Expression<Func<T, bool>> exp);
    }
}
