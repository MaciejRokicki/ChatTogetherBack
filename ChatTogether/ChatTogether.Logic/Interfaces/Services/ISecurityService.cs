using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Ports.Dtos.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface ISecurityService
    {
        Task<(ClaimsPrincipal, UserDbo)> SignInAsync(AccountDto accountDto);
        Task SignUpAsync(AccountDto accountDto, string nickname);

        Task SendConfirmationEmailAsync(string email, bool isNewAccount = true);
        Task ConfirmEmailAsync(string email, string token);

        Task ChangeEmailRequestAsync(string email);
        Task ChangeEmailAsync(string token, string newEmail);

        Task ChangePasswordRequestAsync(string email);
        Task ChangePasswordAsync(string token, string newPassword);

        Task ChangeRoleAsync(int userId, Role role);

        Task<bool> BlockAccountAsync(int userId, string reason, int blockedById, DateTime? blockedTo = null);
        Task UnblockAccountAsync(int userId);
        Task<Page<BlockedAccountDbo>> GetBlockedUsersAsync(int page, int pageSize, string search);
    }
}
