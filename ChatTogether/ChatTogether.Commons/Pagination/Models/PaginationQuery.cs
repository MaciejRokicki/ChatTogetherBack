namespace ChatTogether.Commons.Pagination.Models
{
    public class PaginationQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Filter[] Filters { get; set; }
        public Sorting[] Sortings { get; set; }
    }
}
