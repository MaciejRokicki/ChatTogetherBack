using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<IEnumerable<BlockedAccountDbo>> GetManyAsync()
        {
            List<BlockedAccountDbo> blockedUsers = await chatTogetherDbContext
                .Set<BlockedAccountDbo>()
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
                .ToListAsync();

            return blockedUsers;
        }
    }
}
