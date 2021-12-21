using ChatTogether.Commons.GenericRepository;
using ChatTogether.Commons.Page;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class BlockedAccountRepository : Repository<int, BlockedAccountDbo>, IBlockedAccountRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public BlockedAccountRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<Page<BlockedAccountDbo>> GetPageAsync(int page, int pageSize, string search)
        {
            IQueryable<BlockedAccountDbo> query = chatTogetherDbContext
                .Set<BlockedAccountDbo>()
                .Where(x => x.Account.Email.Contains(search) || x.Account.User.Nickname.Contains(search));

            int count = await query.CountAsync();

            List<BlockedAccountDbo> blockedUsers = await query
                .OrderBy(x => x.Account.Email)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.CreatedBy)
                .ThenInclude(x => x.User)
                .Include(x => x.Account)
                .ThenInclude(x => x.User)
                .Select(x => new BlockedAccountDbo()
                {
                    Account = new AccountDbo()
                    {
                        Email = x.Account.Email,
                        User = new UserDbo()
                        {
                            Id = x.Account.User.Id,
                            Nickname = x.Account.User.Nickname,
                            FirstName = x.Account.User.FirstName,
                            LastName = x.Account.User.LastName
                        }
                    },
                    Reason = x.Reason,
                    BlockedTo = x.BlockedTo,
                    Created = x.Created,
                    CreatedBy = new AccountDbo()
                    {
                        Email = x.CreatedBy.Email,
                        User = new UserDbo()
                        {
                            Nickname = x.CreatedBy.User.Nickname
                        }
                    }
                })
                .ToListAsync();

            int pageCount = (int)Math.Ceiling((float)count / pageSize);

            Page<BlockedAccountDbo> resultPage = new Page<BlockedAccountDbo>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = pageCount > 0 ? pageCount : 1,
                Count = count,
                Data = blockedUsers
            };

            return resultPage;
        }
    }
}
