using ChatTogether.Commons.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatTogether.Commons.Pagination
{
    public static class PaginationExtension
    {
        public static async Task<PaginationPage<T>> GetPaginationPageAsync<T>(this IQueryable<T> query)
        {
            int totalRows = await query.CountAsync();

            IEnumerable<T> data = query
                .AsEnumerable();

            PaginationPage<T> paginationPage = new PaginationPage<T>(0, 0, 0, totalRows, data);

            return paginationPage;

        }
        public static async Task<PaginationPage<T>> GetPaginationPageAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            int totalRows = await query.CountAsync();

            float totalPages = totalRows / pageSize;
            int pages = (int)Math.Ceiling(totalPages);

            List<T> data = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            PaginationPage<T> paginationPage = new PaginationPage<T>(pageSize, page, pages, totalRows, data);

            return paginationPage;
        }
    }
}
