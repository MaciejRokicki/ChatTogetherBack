using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces.Security
{
    public interface IChangePasswordTokenRepository : IRepository<ChangePasswordTokenDbo>
    {
        Task<ChangePasswordTokenDbo> GetWithAccountAsync(Expression<Func<ChangePasswordTokenDbo, bool>> exp);
    }
}
