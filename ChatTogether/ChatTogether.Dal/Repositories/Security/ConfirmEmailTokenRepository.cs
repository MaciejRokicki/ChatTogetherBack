using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;

namespace ChatTogether.Dal.Repositories.Security
{
    public class ConfirmEmailTokenRepository : Repository<ConfirmEmailTokenDbo>, IConfirmEmailTokenRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ConfirmEmailTokenRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }
    }
}
