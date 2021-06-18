using ChatTogether.Dal.Dbos.Security;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Security
{
    public interface IConfirmEmailService
    {
        Task<ConfirmEmailTokenDbo> CreateToken(int accountId);
        Task<bool> CheckToken(int accountId, string token);
        Task DeleteToken(int accountId);
    }
}
