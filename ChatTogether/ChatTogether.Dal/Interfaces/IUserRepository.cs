using ChatTogether.Commons.GenericRepository;
using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces
{
    public interface IUserRepository : IRepository<int, UserDbo>
    {
        Task<bool> IsNicknameAvailable(string nickname);
        Task<UserDbo> GetWithAccountAsync(Expression<Func<UserDbo, bool>> exp);
        Task<Page<UserDbo>> GetPageAsync(int page, int pageSize, string search, Role? role);
    }
}
