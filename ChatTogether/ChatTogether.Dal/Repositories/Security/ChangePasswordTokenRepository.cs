using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories.Security
{
    public class ChangePasswordTokenRepository : Repository<ChangePasswordTokenDbo>, IChangePasswordTokenRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ChangePasswordTokenRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public override async Task<ChangePasswordTokenDbo> GetAsync(Expression<Func<ChangePasswordTokenDbo, bool>> exp)
        {
            ChangePasswordTokenDbo entity = await chatTogetherDbContext
                .Set<ChangePasswordTokenDbo>()
                .AsNoTracking()
                .Include(x => x.Account)
                .FirstOrDefaultAsync(exp);

            return entity;
        }
    }
}
