using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces.Security
{
    public interface IAccountRepository : IRepository<AccountDbo>
    {
        Task<AccountDbo> GetWithUserAsync(Expression<Func<AccountDbo, bool>> exp);
    }
}
