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

        public async Task<Page<BlockedAccountDbo>> GetManyAsync(int page, int pageSize, string search)
        {
            int count = await chatTogetherDbContext
                .Set<BlockedAccountDbo>()
                .CountAsync();

            List<BlockedAccountDbo> blockedUsers = await chatTogetherDbContext
                .Set<BlockedAccountDbo>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Where(x => x.Account.Email.Contains(search) || x.Account.User.Nickname.Contains(search))
                .Include(x => x.Account)
                .ThenInclude(x => x.User)
                .Select(x => new BlockedAccountDbo()
                {
                    Account = new AccountDbo()
                    {
                        Email = x.Account.Email,
                        User = new UserDbo()
                        {
                            Nickname = x.Account.User.Nickname,
                            FirstName = x.Account.User.FirstName,
                            LastName = x.Account.User.LastName
                        }
                    },
                    Reason = x.Reason,
                    BlockedTo = x.BlockedTo,
                    Created = x.Created
                })
                .OrderBy(x => x.Account.Email)
                .ToListAsync();

            Page<BlockedAccountDbo> resultPage = new Page<BlockedAccountDbo>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling((float)count / pageSize),
                Data = blockedUsers
            };

            return resultPage;
        }
    }
}
