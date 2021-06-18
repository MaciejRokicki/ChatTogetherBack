using ChatTogether.Dal.Dbos.Security;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Security
{
    public interface IChangeEmailService
    {
        Task<ChangeEmailTokenDbo> Get(string token);
        Task<ChangeEmailTokenDbo> CreateToken(int accountId);
        Task<bool> CheckToken(int accountId, string token);
        Task DeleteToken(int accountId);
    }
}
