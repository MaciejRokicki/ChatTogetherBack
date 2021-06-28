using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces.Security
{
    public interface IChangeEmailTokenRepository : IRepository<int, ChangeEmailTokenDbo>
    {
        Task<ChangeEmailTokenDbo> GetWithAccountAsync(Expression<Func<ChangeEmailTokenDbo, bool>> exp);
    }
}
