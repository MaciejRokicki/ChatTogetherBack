using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces.Security
{
    public interface IConfirmEmailTokenRepository : IRepository<int, ConfirmEmailTokenDbo>
    {
        Task<ConfirmEmailTokenDbo> GetWithAccountAsync(Expression<Func<ConfirmEmailTokenDbo, bool>> exp);
    }
}
