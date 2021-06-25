using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class AccountRepository : Repository<AccountDbo>, IAccountRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public AccountRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<AccountDbo> GetWithUserAsync(Expression<Func<AccountDbo, bool>> exp)
        {
            AccountDbo accountDbo = await chatTogetherDbContext
                .Set<AccountDbo>()
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return accountDbo;
        }
    }
}
