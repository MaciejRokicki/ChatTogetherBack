using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class ChangeEmailTokenRepository : Repository<int, ChangeEmailTokenDbo>, IChangeEmailTokenRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ChangeEmailTokenRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<ChangeEmailTokenDbo> GetWithAccountAsync(Expression<Func<ChangeEmailTokenDbo, bool>> exp)
        {
            ChangeEmailTokenDbo changeEmailTokenDbo = await chatTogetherDbContext
                .Set<ChangeEmailTokenDbo>()
                .Include(x => x.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return changeEmailTokenDbo;
        }
    }
}
