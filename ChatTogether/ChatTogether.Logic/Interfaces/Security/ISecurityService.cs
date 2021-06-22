using ChatTogether.Ports.Dtos.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Security
{
    public interface ISecurityService
    {
        Task<ClaimsPrincipal> SignIn(AccountDto accountDto);
        Task SignUp(AccountDto accountDto, string nickname);

        Task ResendConfirmationEmail(string email);
        Task ConfirmEmail(string email, string token);

        Task ChangeEmailRequest(string email);
        Task ChangeEmail(string token, string newEmail);

        Task ChangePasswordRequest(string email);
        Task ChangePassword(string token, string newPassword);
    }
}
