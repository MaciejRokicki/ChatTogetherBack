using ChatTogether.Commons.GenericRepository;
using ChatTogether.Commons.Page;
using ChatTogether.Dal.Dbos.Security;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces.Security
{
    public interface IBlockedAccountRepository : IRepository<int, BlockedAccountDbo>
    {
        Task<Page<BlockedAccountDbo>> GetPageAsync(int page, int pageSize, string search);
    }
}
