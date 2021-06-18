using System.Collections.Generic;

namespace ChatTogether.Commons.Pagination.Models
{
    public class PaginationPage<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
        public int TotalRows { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginationPage(int pageSize, int currentPage, int pages, int totalRows, IEnumerable<T> data)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            Pages = pages;
            TotalRows = totalRows;
            Data = data;
        }
    }
}
