using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class ConfirmEmailTokenRepository : Repository<int, ConfirmEmailTokenDbo>, IConfirmEmailTokenRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ConfirmEmailTokenRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<ConfirmEmailTokenDbo> GetWithAccountAsync(Expression<Func<ConfirmEmailTokenDbo, bool>> exp)
        {
            ConfirmEmailTokenDbo confirmEmailTokenDbo = await chatTogetherDbContext
                .Set<ConfirmEmailTokenDbo>()
                .Include(x => x.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return confirmEmailTokenDbo;
        }
    }
}
