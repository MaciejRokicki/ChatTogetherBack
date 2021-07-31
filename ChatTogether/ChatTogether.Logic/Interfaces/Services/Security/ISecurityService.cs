using ChatTogether.Dal.Dbos;
using ChatTogether.Ports.Dtos.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services.Security
{
    public interface ISecurityService
    {
        Task<(ClaimsPrincipal, UserDbo)> SignIn(AccountDto accountDto);
        Task SignUp(AccountDto accountDto, string nickname);

        Task SendConfirmationEmail(string email);
        Task ConfirmEmail(string email, string token);

        Task ChangeEmailRequest(string email);
        Task ChangeEmail(string token, string newEmail);

        Task ChangePasswordRequest(string email);
        Task ChangePassword(string token, string newPassword);
    }
}
