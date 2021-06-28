using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories
{
    public class UserRepository : Repository<int, UserDbo>, IUserRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public UserRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<bool> IsNicknameAvailable(string nickname)
        {
            bool isAvailable = !await chatTogetherDbContext
                .Set<UserDbo>()
                .AsNoTracking()
                .AnyAsync(x => x.Nickname == nickname);

            return isAvailable;
        }

        public async Task<UserDbo> GetWithAccountAsync(Expression<Func<UserDbo, bool>> exp)
        {
            UserDbo userDbo = await chatTogetherDbContext
                .Set<UserDbo>()
                .Include(x => x.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(exp);

            return userDbo;
        }
    }
}
