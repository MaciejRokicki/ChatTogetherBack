using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;

namespace ChatTogether.Dal.Repositories.Security
{
    public class AccountRepository : Repository<AccountDbo>, IAccountRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public AccountRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }
    }
}
