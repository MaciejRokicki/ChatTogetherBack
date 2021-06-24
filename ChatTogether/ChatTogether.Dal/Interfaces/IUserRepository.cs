using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces
{
    public interface IUserRepository : IRepository<UserDbo>
    {
        Task<bool> IsNicknameAvailable(string nickname);
        Task<UserDbo> GetWithAccountAsync(Expression<Func<UserDbo, bool>> exp);
    }
}
