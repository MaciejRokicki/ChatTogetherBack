using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Ports.Dtos.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services.Security
{
    public interface ISecurityService
    {
        Task<(ClaimsPrincipal, UserDbo)> SignIn(AccountDto accountDto);
        Task SignUp(AccountDto accountDto, string nickname);

        Task SendConfirmationEmail(string email, bool isNewAccount = true);
        Task ConfirmEmail(string email, string token);

        Task ChangeEmailRequest(string email);
        Task ChangeEmail(string token, string newEmail);

        Task ChangePasswordRequest(string email);
        Task ChangePassword(string token, string newPassword);

        Task ChangeRole(int userId, Role role);

        Task<bool> BlockAccount(int userId, string reason, int blockedById, DateTime? blockedTo = null);
        Task UnblockAccount(int userId);
        Task<Page<BlockedAccountDbo>> GetBlockedUsers(int page, int pageSize, string search);
    }
}
