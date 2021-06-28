using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class ChangePasswordTokenRepository : Repository<int, ChangePasswordTokenDbo>, IChangePasswordTokenRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ChangePasswordTokenRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<ChangePasswordTokenDbo> GetWithAccountAsync(Expression<Func<ChangePasswordTokenDbo, bool>> exp)
        {
            ChangePasswordTokenDbo changePasswordTokenDbo = await chatTogetherDbContext
                .Set<ChangePasswordTokenDbo>()
                .Include(x => x.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return changePasswordTokenDbo;
        }
    }
}
