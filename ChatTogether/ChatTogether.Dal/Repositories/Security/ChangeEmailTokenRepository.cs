using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class ChangeEmailTokenRepository : Repository<ChangeEmailTokenDbo>, IChangeEmailTokenRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ChangeEmailTokenRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public override async Task<ChangeEmailTokenDbo> GetAsync(Expression<Func<ChangeEmailTokenDbo, bool>> exp)
        {
            ChangeEmailTokenDbo entity = await chatTogetherDbContext
                .Set<ChangeEmailTokenDbo>()
                .AsNoTracking()
                .Include(x => x.Account)
                .FirstOrDefaultAsync(exp);

            return entity;
        }
    }
}
