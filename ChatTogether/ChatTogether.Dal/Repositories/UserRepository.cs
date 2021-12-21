using ChatTogether.Commons.GenericRepository;
using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Page<UserDbo>> GetPageAsync(int page, int pageSize, string search, Role? role)
        {
            IQueryable<UserDbo> query = chatTogetherDbContext
                .Set<UserDbo>();
                
            if (role != null)
            {
                query = query.Where(x => x.Account.Role == role);
            }

            query = query.Where(x => x.Nickname.Contains(search) || x.FirstName.Contains(search) || x.LastName.Contains(search));

            int count = await query.CountAsync();

            List<UserDbo> users = await query
                .OrderBy(x => x.Nickname)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)          
                .ToListAsync();

            int pageCount = (int)Math.Ceiling((float)count / pageSize);

            Page<UserDbo> resultPage = new Page<UserDbo>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = pageCount > 0 ? pageCount : 1,
                Count = count,
                Data = users
            };

            return resultPage;
        }
    }
}
