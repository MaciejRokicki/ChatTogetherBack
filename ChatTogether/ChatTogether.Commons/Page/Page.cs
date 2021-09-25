using System.Collections.Generic;

namespace ChatTogether.Commons.Page
{
    public class Page<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
