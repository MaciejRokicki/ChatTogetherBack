using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;

namespace ChatTogether.Dal.Interfaces.Security
{
    public interface IBlockedAccountRepository : IRepository<int, BlockedAccountDbo>
    {

    }
}
