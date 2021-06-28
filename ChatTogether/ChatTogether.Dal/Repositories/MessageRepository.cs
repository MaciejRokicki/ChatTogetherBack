using ChatTogether.Commons.GenericRepository;
using ChatTogether.Commons.Pagination;
using ChatTogether.Commons.Pagination.Models;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories
{
    public class MessageRepository : Repository<Guid, MessageDbo>, IMessageRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public MessageRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public override async Task<PaginationPage<MessageDbo>> GetPageAsync(PaginationQuery paginationQuery)
        {
            IQueryable<MessageDbo> query = chatTogetherDbContext
                .Set<MessageDbo>()
                .Include(x => x.User);

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

            PaginationPage<MessageDbo> paginationPage = await query.GetPaginationPageAsync(paginationQuery.Page, paginationQuery.PageSize);

            return paginationPage;
        }
    }
}
